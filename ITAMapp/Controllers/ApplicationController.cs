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
           var categoryId = (from categories in _context.Categories
                          where categories.UrlName == category
                          select categories.CategoryId).FirstOrDefault();
            //Gets a list of the unique identification IDs for this category
            var uniqueIdentifications = (from identifications in _context.Identifications
                                         where identifications.CategoryId == categoryId
                                         select identifications).ToList();

            //Creates an AssetViewModel for each identification and adds it to a list
            foreach (var uniqueItem in uniqueIdentifications)
            {
                assetList.Add(createAsset(uniqueItem.IdentificationId));

            }

            //Queries the list of fields for this category
            var labelList = (from label in _context.CustomFields
                             where label.CategoryId == categoryId
                             select label.FieldLabel).ToList();

            //Creates a new CategoryViewModel and returns it
            CategoryViewModel newCategory = new CategoryViewModel();
            newCategory.assets = assetList;
            newCategory.labels = labelList;
            newCategory.urlCategory = category;
            return View(newCategory);

        }


        public IActionResult Add (string category)
        {
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

            //Creates a new AddAssetViewModel and returns it
            AddAssetViewModel addAsset = new AddAssetViewModel();
            addAsset.fields = properties;
            addAsset.dropdownFields = dropdownFields;
            addAsset.urlCategory = category;
            return View(addAsset);
            
        }

        [HttpPost]
        public IActionResult insert(string category)
        {
            var categoryId = (from categories in _context.Categories
                          where categories.UrlName == category
                          select categories.CategoryId).FirstOrDefault();

            List<string> stuff = new List<string>();
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
                stuff.Add(Request.Form[label.FieldLabel].ToString());

            }

            //Gets the field that will used to uniquely identify this category of asset
            string identifierLabel = Request.Form["identityLabelId"];

            //Gets the value of the unique identifier label
            string identifier = Request.Form[identifierLabel];

            //Adds this new identification to the database
            _context.Identifications.Add(new Identifications { CategoryId = categoryId, Identification = identifier });
            _context.SaveChanges();

            //Gets the identification ID
            int identificationId = (from identifications in _context.Identifications
                                    where identifications.Identification == identifier
                                    select identifications.IdentificationId).SingleOrDefault();

            //Adds each field value from the form into the database
            foreach (var label in labelList)
            {
                string formValue = Request.Form[label.FieldId.ToString()];
                if(formValue == "true,false")
                {
                    formValue = "Yes";
                }
                if(formValue == "false")
                {
                    formValue = "No";
                }
                _context.FieldValues.Add(new FieldValues { FieldId = label.FieldId, FieldValue = formValue, IdentificationId = identificationId });
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string category, int id)
        {
            //Queries the list of dropdown values
            List<CustomFieldList> dropdownFields = (from items in _context.CustomFieldList
                                                    select new CustomFieldList
                                                    {
                                                        ListId = items.ListId,
                                                        FieldId = items.FieldId,
                                                        ListDescription = items.ListDescription

                                                    }).ToList();

            //Creates a list that joins a field's data with its value into a FieldInfo object
            List<FieldInfo> fieldsList = (from fields in _context.CustomFields
                                join values in _context.FieldValues on fields.FieldId equals values.FieldId
                                where values.IdentificationId == id
                                select new FieldInfo
                                {
                                    FieldId = fields.FieldId,
                                    FieldLabel = fields.FieldLabel,
                                    FieldType = fields.FieldType,
                                    IsIdentifier = fields.IsIdentifier,
                                    FieldValue = values.FieldValue,
                                    CollapsedBy = fields.CollapsedBy

                                }).ToList();

            //Creates a new EditAssetViewModel and populates it
            EditAssetViewModel newViewModel = new EditAssetViewModel();
            newViewModel.dropdownFields = dropdownFields;
            newViewModel.fields = fieldsList;
            newViewModel.identifier = id;
            newViewModel.urlCategory = category;

            return View(newViewModel);
        }
        [HttpPost]
        public IActionResult update()
        {
            //Gets all the field values from the database that belong to the chosen identification
            List<FieldValues> values = (from value in _context.FieldValues
                          where value.IdentificationId == int.Parse(Request.Form["identifier"])
                          select value).ToList();

            foreach (var value in values)
            {
                string formValue = Request.Form[value.FieldId.ToString()];
                if (formValue == "true,false")
                {
                    formValue = "Yes";
                }
                if (formValue == "false")
                {
                    formValue = "No";
                }
                value.FieldValue = formValue;
            }

            _context.SaveChanges();

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

            //Creates a PropertyViewModel for each Property object queried and adds it to alist
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
            AssetViewModel newAsset = new AssetViewModel();
            newAsset.fields = propertyList;
            newAsset.identification = identificationID;
            return newAsset;
        }
    }
}
