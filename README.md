# ITAMapp (Readme incomplete
ITAMapp is an inventory management web application that lets you organize the inventory based on how you structure the database. 
## Getting Started
### Prerequisites
* Microsoft Visual Studio 2017
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
* This is done in the CustomFields tables. Note that the order of the fields will be dependent on the order in which you add them to the database
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
