# ITAMapp
ITAMapp is an inventory management web application that lets you organize the inventory based on how you structure the database. 
## Getting Started
### Prerequisites
* Microsoft Visual Studio 2017 with the following workloads installed:
  * Universal Windows Platform development
  * .NET desktop development
  * ASP.NET and web development
  * Data storage and processing
  * .NET Core cross-platform development
* Github extension for Visual Studio
### Installing With Github
1. Clone this repository 
2. In Visual Studio, go to File -> New -> Project From Existing Code
3. Select C# for the project type
4. Browse to the folder that contains the project files and then give the project a name
### Installing through Visual Studio
1. Fork this repository
2. Create a new C# console application in Visual Studio
3. Go to the Team Explorer tab 
4. Under the Github heading, press Clone
5. Select this repository to clone it into Visual Studio
## Setting up the database contents
### Adding Categories
* In the Categories table, the CategoryDescription column will be the name of the category that appears on the menu
* The UrlName column will be the url extension that leads to that category and will also be used in the titles(Add New *UrlName*)
### Adding Fields for Categories
* This is done in the CustomFields table. Note that the order of the fields will be dependent on the order in which you add them to the database
* The CategoryId will be the id of the corresponding category for this field
* The FieldLabel column will be the name of the field
* The FieldType column determines what kind of field it will be depending on the number
  * 0 - Regular text input field
  * 1 - Dropdown field
  * 2 - Checkbox
  * 3 - Date field
  * 4 - Textbox input field
  * 5 - Multiselect field
* The isIdentifier column determines if this field will be used to uniquely identify an item in this category
* The CollapsedBy column is used for subfields that belong to a checkbox field. This value will be the FieldId for the parent field
### Adding Items for Dropdown and Multiselect fields
* This is done in the CustomFieldsList table. Like the CustomFields table, the order of the dropdown items is dependent on the order they were added to the database
* The FieldId column will contain the ID of the field this item belongs to
* The ListDescription column will be the name of the item in the dropdown or multiselect list
