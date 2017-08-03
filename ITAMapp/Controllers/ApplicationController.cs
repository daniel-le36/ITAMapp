using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITAMapp.ViewModels;
using ITAMapp.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITAMapp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ITAMContext _context;

        public ApplicationController(ITAMContext context)

        {
            _context = context;
            
        }
        // GET: /<controller>/
        public IActionResult Index(string category)
        {
            List<AssetViewModel> assetList = new List<AssetViewModel>();

            //Gets the ID of the current category
            var categoryId = (from categories in _context.Categories
                          where categories.UrlName == category
                          select categories.CategoryId).FirstOrDefault();

            //Gets a list of the unique identification IDs for this category
            var uniqueIdentifications = (from identifications in _context.Identifications
                                         where identifications.CategoryId == categoryId
                                         select identifications).ToList();

            //Creates an AssetViewModel for each identification using the private createAsset method and adds it to a list
            foreach (var uniqueItem in uniqueIdentifications)
            {
                assetList.Add(createAsset(uniqueItem.IdentificationId));

            }

            //Queries the list of fields for this category
            var labelList = (from label in _context.CustomFields
                             where label.CategoryId == categoryId
                             select label.FieldLabel).ToList();

            //Creates a new CategoryViewModel which contains the category, list of assets, and field names then returns it
            return View(new CategoryViewModel { assets = assetList, labels = labelList, urlCategory = category});

        }

        public IActionResult Add (string category)
        {

            //Gets the ID of the current category
            var categoryId = (from categories in _context.Categories
                          where categories.UrlName == category
                          select categories.CategoryId).FirstOrDefault();

            //Queries the list of fields for this category
            List<CustomFields> properties = (from label in _context.CustomFields
                              where label.CategoryId == categoryId
                              select new CustomFields
                              {
                                  FieldId = label.FieldId,
                                  CategoryId = label.CategoryId,
                                  FieldLabel = label.FieldLabel,
                                  FieldType = label.FieldType,
                                  CollapsedBy = label.CollapsedBy,
                                  IsIdentifier = label.IsIdentifier

                             }).ToList();

            //Queries the list of dropdown values
            List<CustomFieldList> dropdownFields = (from items in _context.CustomFieldList
                                                    select new CustomFieldList
                                                    {
                                                        ListId = items.ListId,
                                                        FieldId = items.FieldId,
                                                        ListDescription = items.ListDescription

                                                    }).ToList();

            //Creates a new AddAssetViewModel which contains the list of fields, field values for dropdown fields, and the category then returns it
            return View(new AddAssetViewModel { fields = properties, dropdownFields = dropdownFields, urlCategory = category});
            
        }

        [HttpPost]
        public IActionResult insert(string category)
        {

            //Gets the ID of the current category
            var categoryId = (from categories in _context.Categories
                          where categories.UrlName == category
                          select categories.CategoryId).FirstOrDefault();

            //Creates a list that will hold all the field names for this category
            List<string> fieldList = new List<string>();
            
            //Gets the list of fields for this category
            var labelList = (from label in _context.CustomFields
                             where label.CategoryId == categoryId
                             select new CustomFields
                             {
                                 FieldId = label.FieldId,
                                 FieldLabel = label.FieldLabel
                             }).ToList();

            //Adds each field value from the form into the database
            foreach (var label in labelList)
            {
                fieldList.Add(Request.Form[label.FieldLabel].ToString());

            }

            //Gets the field that will used to uniquely identify this category of asset
            string identifierLabel = Request.Form["identityLabelId"];

            //Gets the value of the unique identifier label
            string identifier = Request.Form[identifierLabel];

            //Adds this new identification to the database
            _context.Identifications.Add(new Identifications { CategoryId = categoryId, Identification = identifier });

            //Saves changes made to the database
            _context.SaveChanges();

            //Gets the recently added identification ID
            int identificationId = (from identifications in _context.Identifications
                                    where identifications.Identification == identifier
                                    select identifications.IdentificationId).SingleOrDefault();

            //Adds each field value from the form into the database
            foreach (var label in labelList)
            {

                //Gets the field value from the form
                string formValue = Request.Form[label.FieldId.ToString()];

                //Changes checked checkbox value to Yes
                if(formValue == "true,false")
                {
                    formValue = "Yes";
                }

                //Changes unchecked checkbox value to No
                if(formValue == "false")
                {
                    formValue = "No";
                }

                //Creates a new FieldValue object and adds it to the database
                _context.FieldValues.Add(new FieldValues { FieldId = label.FieldId, FieldValue = formValue, IdentificationId = identificationId });

                //Saves changes made to the database
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string category, int id)
        {
            //Queries the list of dropdown fields
            List<CustomFieldList> dropdownFields = (from items in _context.CustomFieldList
                                                    select new CustomFieldList
                                                    {
                                                        ListId = items.ListId,
                                                        FieldId = items.FieldId,
                                                        ListDescription = items.ListDescription

                                                    }).ToList();

            //Gets the ID of the current category
            var categoryId = (from categories in _context.Categories
                              where categories.UrlName == category
                              select categories.CategoryId).FirstOrDefault();

            //Queries the list of fields for this category and inputs their info into a FieldInfo object
            //This object is almost identical to a CustomFields object except it can also store the value of the field
            List<FieldInfo> properties = (from label in _context.CustomFields
                                             where label.CategoryId == categoryId
                                             select new FieldInfo
                                             {
                                                 FieldId = label.FieldId,
                                                 FieldLabel = label.FieldLabel,
                                                 FieldType = label.FieldType,
                                                 CollapsedBy = label.CollapsedBy,
                                                 IsIdentifier = label.IsIdentifier
                                             }).ToList();

            //For each field, inserts the value corresponding to the field value for the current item
            foreach(var field in properties)
            {
                //Gets the value for this item's field 
                var fieldValue = (from values in _context.FieldValues
                                  where values.FieldId == field.FieldId && values.IdentificationId == id
                                  select values.FieldValue).FirstOrDefault();

                //Sets the value of the field to the item's field
                field.FieldValue = fieldValue;
                
            }

            //Creates a new EditAssetViewModel which contains the list of dropdown values, list fields with their values, the id, and category then returns it
            return View(new EditAssetViewModel { dropdownFields = dropdownFields, fields = properties, identifier = id, urlCategory = category});
        }
        [HttpPost]
        public IActionResult update(string category)
        {
            //Gets the ID of the current category
            var categoryId = (from categories in _context.Categories
                              where categories.UrlName == category
                              select categories.CategoryId).FirstOrDefault();

            //Creates a list that will hold the field IDs for this category
            List<int> fields = (from fieldList in _context.CustomFields
                          where fieldList.CategoryId == categoryId
                          select fieldList.FieldId).ToList();

            //Gets the identification number of the item from the form
            int identification = int.Parse(Request.Form["identifier"]);

            //For each field in this category, updates the current field value or adds a new field value if one does not exist
            foreach(var fieldId in fields)
            {

                //Gets the value of the current item's field
                var valueForId = (from fieldVal in _context.FieldValues
                                  where fieldVal.FieldId == fieldId && fieldVal.IdentificationId == identification
                                  select fieldVal.FieldValue).FirstOrDefault();

                //Gets the value of the current field from the form
                string formValue = Request.Form[fieldId.ToString()];

                //Changes checked checkbox value to Yes
                if (formValue == "true,false")
                {
                    formValue = "Yes";
                }

                //Changes unchecked checkbox value to No
                if (formValue == "false")
                {
                    formValue = "No";
                }

                //Adds a new FieldValue to the database if a new field was added after this item was added to the database
                if(valueForId == null)
                {
                    _context.FieldValues.Add(new FieldValues { FieldId = fieldId, IdentificationId = identification, FieldValue = formValue });

                }

                //Updates the field's current value
                else
                {
                    var currentVal = (from value in _context.FieldValues
                                      where value.IdentificationId == identification && value.FieldId == fieldId
                                      select value).FirstOrDefault();
                    currentVal.FieldValue = formValue;
                }

                //Saves changes made to the database
                _context.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public IActionResult delete(int id)
        {
            //Queries the list of current field values for the given asset
            List<FieldValues> values = (from value in _context.FieldValues
                                        where value.IdentificationId == id
                                        select value).ToList();

            //Removes all these values from the database
            _context.FieldValues.RemoveRange(values);

            //Queries the identification from the database for this asset
            var identification = (from identity in _context.Identifications
                                        where identity.IdentificationId == id
                                        select identity).FirstOrDefault();

            //Removes the identification from the database and saves the changes
            _context.Identifications.Remove(identification);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //Private method to create and return an AssetViewModel corresponding to the given identification ID
        private AssetViewModel createAsset(int identificationID)
        {
            //Creates list of PropertyViewModels which contain the category name, identification, field name, and the value of the field
            List<PropertyViewModel> propertyList = new List<PropertyViewModel>();

            //Queries a list of the property values for this identification
            var queryData = (from category in _context.Categories
                             join identification in _context.Identifications on category.CategoryId equals identification.CategoryId
                             join fields in _context.CustomFields on identification.CategoryId equals fields.CategoryId
                             join fieldValue in _context.FieldValues on new { fields.FieldId, identification.IdentificationId } equals new { fieldValue.FieldId, fieldValue.IdentificationId }
                             where identification.IdentificationId == identificationID
                             select new
                             {
                                 category.CategoryDescription,
                                 identification.Identification,
                                 fields.FieldLabel,
                                 fieldValue.FieldValue
                             }
                        ).ToList();

            //Creates a PropertyViewModel for each Property object queried and adds it to a list
            foreach (var item in queryData)
            {
                propertyList.Add(new PropertyViewModel()
                {
                    CategoryDescription = item.CategoryDescription,
                    Identification = item.Identification,
                    FieldLabel = item.FieldLabel,
                    FieldProperty = item.FieldValue
                });
            }

            //Creates the AssetViewModel and returns it
            return new AssetViewModel {fields = propertyList, identification = identificationID } ;
        }
    }
}
