using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SharePointSampleDataGenerator {

  class SharePointListFactory {

    #region "Variables to track ClientContext, Site and Web"

    static string siteUrl = ConfigurationManager.AppSettings["targetSiteUrl"];
    static ClientContext clientContext = new ClientContext(siteUrl);
    static Site siteCollection = clientContext.Site;
    static Web site = clientContext.Web;

    static SharePointListFactory() {
      string userName = ConfigurationManager.AppSettings["userName"];
      string password = ConfigurationManager.AppSettings["password"];
      SecureString securePassword = new SecureString();
      foreach (char c in password) {
        securePassword.AppendChar(c);
      };
      clientContext.Credentials = new SharePointOnlineCredentials(userName, securePassword);
      clientContext.Load(site);
      clientContext.Load(site.Lists);
      clientContext.Load(site.ContentTypes);
      clientContext.ExecuteQuery();
    }

    #endregion

    #region "Variables and helper methods for site columns, content types and lists"

    static Field CreateSiteColumn(string fieldName, string fieldDisplayName, string fieldType) {

      Console.WriteLine("Creating " + fieldName + " site column...");

      // delete existing field if it exists
      try {
        Field fld = site.Fields.GetByInternalNameOrTitle(fieldName);
        fld.DeleteObject();
        clientContext.ExecuteQuery();
      }
      catch { }

      string fieldXML = @"<Field Name='" + fieldName + "' " +
                                "DisplayName='" + fieldDisplayName + "' " +
                                "Type='" + fieldType + "' " +
                                "Group='Critical Path Training' > " +
                         "</Field>";

      Field field = site.Fields.AddFieldAsXml(fieldXML, true, AddFieldOptions.DefaultValue);
      clientContext.Load(field);
      clientContext.ExecuteQuery();
      return field;
    }

    static void DeleteContentType(string contentTypeName) {
      
      try {
        foreach (var ct in site.ContentTypes) {
          if (ct.Name.Equals(contentTypeName)) {
            ct.DeleteObject();
            Console.WriteLine("Deleting existing " + ct.Name + " content type...");
            clientContext.ExecuteQuery();
            break;
          }
        }
      }
      catch { }

    }

    static ContentType CreateContentType(string contentTypeName, string baseContentType) {

      DeleteContentType(contentTypeName);

      ContentTypeCreationInformation contentTypeCreateInfo = new ContentTypeCreationInformation();
      contentTypeCreateInfo.Name = contentTypeName;
      contentTypeCreateInfo.ParentContentType = site.ContentTypes.GetById(baseContentType); ;
      contentTypeCreateInfo.Group = "Critical Path Training";
      ContentType ctype = site.ContentTypes.Add(contentTypeCreateInfo);
      clientContext.ExecuteQuery();
      return ctype;

    }

    static void DeleteList(string listTitle) {
      try {
        List list = site.Lists.GetByTitle(listTitle);
        list.DeleteObject();
        Console.WriteLine("Deleting existing " + listTitle + " list...");
        clientContext.ExecuteQuery();
      }
      catch { }
    }

    #endregion

    #region "Products List"

    static Field fieldProductCode;
    static FieldMultiLineText fieldProductDescription;
    static FieldCurrency fieldProductListPrice;
    static FieldMultiChoice fieldProductColor;
    static FieldNumber fieldMinimumAge;
    static FieldNumber fieldMaximumAge;
    static FieldUrl fieldProductImageUrl;

    static ContentType ctypeProduct;

    static List listProducts;
    static List listProductImages;
    static string listProductImagesUrl;

    static public void CreateProductsLists() {

      try {
        clientContext.Load(siteCollection);
        clientContext.Load(site);
        clientContext.Load(site.Fields);
        clientContext.Load(site.ContentTypes);
        clientContext.ExecuteQuery();

        DeleteProductListTypes();
        CreateProductImagesLibrary();
        UploadProductImages();
        CreateSiteColumns();
        CreateProductContentType();
        CreateProductsList();
        PopulateProductsList();

        Console.WriteLine("The Products list and its dependant types have been created.");
        Console.WriteLine();
      }
      catch (Exception ex) {
        Console.WriteLine();
        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine("----  Error occured when attempting to create Products list ----");
        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine("Error type:");
        Console.WriteLine(ex.GetType().ToString());
        Console.WriteLine();
        Console.WriteLine("Error message:");
        Console.WriteLine(ex.Message);
        Console.WriteLine();
        Console.WriteLine("You must find and correct the problem ou are experiencing in order to successfully use this utility.");
      }
    }

    static void DeleteProductListTypes() {
      DeleteList("Product Images");
      DeleteList("Products");
      DeleteContentType("Product");
    }

    static void CreateSiteColumns() {

      Console.WriteLine();

      fieldProductCode = CreateSiteColumn("ProductCode", "Product Code", "Text");
      fieldProductCode.EnforceUniqueValues = true;
      fieldProductCode.Indexed = true;
      fieldProductCode.Required = true;
      fieldProductCode.Update();
      clientContext.ExecuteQuery();
      clientContext.Load(fieldProductCode);
      clientContext.ExecuteQuery();

      fieldProductDescription = clientContext.CastTo<FieldMultiLineText>(CreateSiteColumn("ProductDescription", "Product Description", "Note"));
      fieldProductDescription.NumberOfLines = 4;
      fieldProductDescription.RichText = false;
      fieldProductDescription.Update();
      clientContext.ExecuteQuery();

      fieldProductListPrice = clientContext.CastTo<FieldCurrency>(CreateSiteColumn("ProductListPrice", "List Price", "Currency"));
      fieldProductListPrice.MinimumValue = 0;
      fieldProductListPrice.Update();
      clientContext.ExecuteQuery();

      fieldProductColor = clientContext.CastTo<FieldMultiChoice>(CreateSiteColumn("ProductColor", "Product Color", "MultiChoice"));
      string[] choicesProductColor = { "White", "Black", "Grey", "Blue", "Red", "Green", "Yellow" };
      fieldProductColor.Choices = choicesProductColor;
      fieldProductColor.Update();
      clientContext.ExecuteQuery();

      fieldMinimumAge = clientContext.CastTo<FieldNumber>(CreateSiteColumn("MinimumAge", "Minimum Age", "Number"));
      fieldMinimumAge.MinimumValue = 1;
      fieldMinimumAge.MaximumValue = 100;
      fieldMinimumAge.Update();
      clientContext.ExecuteQuery();

      fieldMaximumAge = clientContext.CastTo<FieldNumber>(CreateSiteColumn("MaximumAge", "Maximum Age", "Number"));
      fieldMaximumAge.MinimumValue = 1;
      fieldMaximumAge.MaximumValue = 100;
      fieldMaximumAge.Update();
      clientContext.ExecuteQuery();

      fieldProductImageUrl = clientContext.CastTo<FieldUrl>(CreateSiteColumn("ProductImageUrl", "Product Image Url", "URL"));
      fieldProductImageUrl.DisplayFormat = UrlFieldFormatType.Image;
      fieldProductImageUrl.Update();
      clientContext.ExecuteQuery();


    }

    static void CreateProductImagesLibrary() {
      Console.WriteLine();
      Console.WriteLine("Creating Product Images library...");


      ListCreationInformation listInformationProductImages = new ListCreationInformation();
      listInformationProductImages.Title = "Product Images";
      listInformationProductImages.Url = "ProductImages";
      listInformationProductImages.QuickLaunchOption = QuickLaunchOptions.On;
      listInformationProductImages.TemplateType = (int)ListTemplateType.PictureLibrary;
      listProductImages = site.Lists.Add(listInformationProductImages);
      listProductImages.OnQuickLaunch = true;
      listProductImages.Update();

      clientContext.ExecuteQuery();

      listProductImagesUrl = site.Url + "/ProductImages/";

    }

    static void UploadProductImage(byte[] imageContent, string imageFileName) {

      Console.WriteLine("  uploading " + imageFileName);


      FileCreationInformation fileInfo = new FileCreationInformation();
      fileInfo.Content = imageContent;
      fileInfo.Overwrite = true;
      fileInfo.Url = listProductImagesUrl + imageFileName;
      File newFile = listProductImages.RootFolder.Files.Add(fileInfo);
      clientContext.ExecuteQuery();

    }

    static void UploadProductImages() {

      Console.WriteLine();
      Console.WriteLine("Uploading Product Images...");

      UploadProductImage(Properties.Resources.WP0001, "WP0001.jpg");
      UploadProductImage(Properties.Resources.WP0002, "WP0002.jpg");
      UploadProductImage(Properties.Resources.WP0003, "WP0003.jpg");
      UploadProductImage(Properties.Resources.WP0004, "WP0004.jpg");
      UploadProductImage(Properties.Resources.WP0005, "WP0005.jpg");
      UploadProductImage(Properties.Resources.WP0006, "WP0006.jpg");
      UploadProductImage(Properties.Resources.WP0007, "WP0007.jpg");
      UploadProductImage(Properties.Resources.WP0008, "WP0008.jpg");
      UploadProductImage(Properties.Resources.WP0009, "WP0009.jpg");
      UploadProductImage(Properties.Resources.WP0010, "WP0010.jpg");
      UploadProductImage(Properties.Resources.WP0011, "WP0011.jpg");
      UploadProductImage(Properties.Resources.WP0012, "WP0012.jpg");
      UploadProductImage(Properties.Resources.WP0013, "WP0013.jpg");
      UploadProductImage(Properties.Resources.WP0014, "WP0014.jpg");
      UploadProductImage(Properties.Resources.WP0015, "WP0015.jpg");
      UploadProductImage(Properties.Resources.WP0016, "WP0016.jpg");
      UploadProductImage(Properties.Resources.WP0017, "WP0017.jpg");
      UploadProductImage(Properties.Resources.WP0018, "WP0018.jpg");
      UploadProductImage(Properties.Resources.WP0019, "WP0019.jpg");
      UploadProductImage(Properties.Resources.WP0020, "WP0020.jpg");
      UploadProductImage(Properties.Resources.WP0021, "WP0021.jpg");
      UploadProductImage(Properties.Resources.WP0022, "WP0022.jpg");
      UploadProductImage(Properties.Resources.WP0023, "WP0023.jpg");
      UploadProductImage(Properties.Resources.WP0024, "WP0024.jpg");
      UploadProductImage(Properties.Resources.WP0025, "WP0025.jpg");
      UploadProductImage(Properties.Resources.WP0026, "WP0026.jpg");
      UploadProductImage(Properties.Resources.WP0027, "WP0027.jpg");
      UploadProductImage(Properties.Resources.WP0028, "WP0028.jpg");
      UploadProductImage(Properties.Resources.WP0029, "WP0029.jpg");
      UploadProductImage(Properties.Resources.WP0030, "WP0030.jpg");
      UploadProductImage(Properties.Resources.WP0031, "WP0031.jpg");
      UploadProductImage(Properties.Resources.WP0032, "WP0032.jpg");

    }

    static void CreateProductContentType() {

      Console.WriteLine("Creating Product content type...");
      ctypeProduct = CreateContentType("Product", "0x01");

      // add site columns
      FieldLinkCreationInformation fieldLinkProductCode = new FieldLinkCreationInformation();
      fieldLinkProductCode.Field = fieldProductCode;
      ctypeProduct.FieldLinks.Add(fieldLinkProductCode);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkProductDescription = new FieldLinkCreationInformation();
      fieldLinkProductDescription.Field = fieldProductDescription;
      ctypeProduct.FieldLinks.Add(fieldLinkProductDescription);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkProductListPrice = new FieldLinkCreationInformation();
      fieldLinkProductListPrice.Field = fieldProductListPrice;
      ctypeProduct.FieldLinks.Add(fieldLinkProductListPrice);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkProductColor = new FieldLinkCreationInformation();
      fieldLinkProductColor.Field = fieldProductColor;
      ctypeProduct.FieldLinks.Add(fieldLinkProductColor);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkMinimumAge = new FieldLinkCreationInformation();
      fieldLinkMinimumAge.Field = fieldMinimumAge;
      ctypeProduct.FieldLinks.Add(fieldLinkMinimumAge);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkMaximumAge = new FieldLinkCreationInformation();
      fieldLinkMaximumAge.Field = fieldMaximumAge;
      ctypeProduct.FieldLinks.Add(fieldLinkMaximumAge);
      ctypeProduct.Update(true);

      FieldLinkCreationInformation fieldLinkProductImageUrl = new FieldLinkCreationInformation();
      fieldLinkProductImageUrl.Field = fieldProductImageUrl;
      ctypeProduct.FieldLinks.Add(fieldLinkProductImageUrl);
      ctypeProduct.Update(true);

      clientContext.ExecuteQuery();

    }

    static void CreateProductsList() {

      Console.WriteLine("Creating Products list...");

      ListCreationInformation listInformationProducts = new ListCreationInformation();
      listInformationProducts.Title = "Products";
      listInformationProducts.Url = "Lists/Products";
      listInformationProducts.QuickLaunchOption = QuickLaunchOptions.On;
      listInformationProducts.TemplateType = (int)ListTemplateType.GenericList;
      listProducts = site.Lists.Add(listInformationProducts);
      listProducts.OnQuickLaunch = true;
      listProducts.Update();

      clientContext.Load(listProducts);
      clientContext.Load(listProducts.ContentTypes);
      clientContext.ExecuteQuery();

      listProducts.ContentTypesEnabled = true;
      listProducts.ContentTypes.AddExistingContentType(ctypeProduct);
      ContentType existing = listProducts.ContentTypes[0]; ;
      existing.DeleteObject();
      listProducts.Update();
      clientContext.ExecuteQuery();

      View viewProducts = listProducts.DefaultView;
      viewProducts.ViewFields.Add("ProductCode");
      viewProducts.ViewFields.Add("ProductListPrice");
      viewProducts.ViewFields.Add("ProductColor");
      viewProducts.Update();

      clientContext.ExecuteQuery();

      string fieldXML = @"<Field 
                            Type=""Calculated""
                            Name=""AgeRange""
                            DisplayName=""Age Range"" 
                            EnforceUniqueValues=""FALSE"" 
                            Indexed=""FALSE"" 
                            ResultType=""Text"" > 
                            <Formula>=IF(AND(ISBLANK([Minimum Age]),ISBLANK([Maximum Age])),&quot;All ages&quot;,IF(ISBLANK([Maximum Age]),&quot;Ages &quot;&amp;[Minimum Age]&amp;&quot; and older&quot;,IF(ISBLANK([Minimum Age]),&quot;Ages &quot;&amp;[Maximum Age]&amp;&quot; and younger&quot;,&quot;Ages &quot;&amp;[Minimum Age]&amp;&quot; to &quot;&amp;[Maximum Age])))</Formula>
                            <FieldRefs>
                              <FieldRef Name=""MinimumAge""/>
                              <FieldRef Name=""MaximumAge""/>
                            </FieldRefs>
                          </Field>";

      listProducts.Fields.AddFieldAsXml(fieldXML, true, AddFieldOptions.DefaultValue);
      clientContext.ExecuteQuery();

      viewProducts.Update();

      clientContext.ExecuteQuery();

      viewProducts.ViewFields.Add("ProductDescription");
      viewProducts.ViewFields.Add("ProductImageUrl");
      viewProducts.Update();

      clientContext.ExecuteQuery();

    }

    static void PopulateProductsList() {

      Console.WriteLine();
      Console.WriteLine("Adding sample items to Products list");
      Console.WriteLine("------------------------------------");
      Console.WriteLine();


      // add a few sample products
      ListItem product1 = listProducts.AddItem(new ListItemCreationInformation());
      product1["Title"] = "Batman Action Figure";
      product1["ProductCode"] = "WP0001";
      product1["ProductListPrice"] = 14.95;
      product1["ProductDescription"] = "A super hero who sometimes plays the role of a dark knight.";
      product1["ProductColor"] = new string[] { "Black" };
      product1["MinimumAge"] = 7;
      product1["MaximumAge"] = 12;
      product1["ProductImageUrl"] = GetFieldUrlValue("WP0001", "Batman Action Figure");
      product1.Update();
      Console.WriteLine("  Adding Batman Action Figure");
      clientContext.ExecuteQuery();


      ListItem product2 = listProducts.AddItem(new ListItemCreationInformation());
      product2["Title"] = "Captain America Action Figure";
      product2["ProductCode"] = "WP0002";
      product2["ProductListPrice"] = 12.95;
      product2["ProductDescription"] = "A super action figure that protects freedom and the American way of life.";
      product2["ProductColor"] = new string[] { "Red", "White", "Blue" };
      product2["MinimumAge"] = 7;
      product2["MaximumAge"] = 12;
      product2["ProductImageUrl"] = GetFieldUrlValue("WP0002", "Captain America Action Figure");
      product2.Update();
      Console.WriteLine("  Adding Captain America Action Figure");
      clientContext.ExecuteQuery();

      ListItem product3 = listProducts.AddItem(new ListItemCreationInformation());
      product3["Title"] = "GI Joe Action Figure";
      product3["ProductCode"] = "WP0003";
      product3["ProductListPrice"] = 14.95;
      product3["ProductDescription"] = "A classic action figure from the 1970s.";
      product3["ProductColor"] = new string[] { "Green" };
      product3["MinimumAge"] = null;
      product3["MaximumAge"] = null;
      product3["ProductImageUrl"] = GetFieldUrlValue("WP0003", "GI Joe Action Figure");
      product3.Update();
      Console.WriteLine("  Adding GI Joe Action Figure");
      clientContext.ExecuteQuery();

      ListItem product4 = listProducts.AddItem(new ListItemCreationInformation());
      product4["Title"] = "Green Hulk Action Figure";
      product4["ProductCode"] = "WP0004";
      product4["ProductListPrice"] = 9.99;
      product4["ProductDescription"] = "An overly muscular action figure that strips naked when angry.";
      product4["ProductColor"] = "Green";
      product4["MinimumAge"] = 7;
      product4["MaximumAge"] = 12;
      product4["ProductImageUrl"] = GetFieldUrlValue("WP0004", "Green Hulk Action Figure");
      product4.Update();
      Console.WriteLine("  Adding Green Hulk Action Figure");
      clientContext.ExecuteQuery();

      ListItem product5 = listProducts.AddItem(new ListItemCreationInformation());
      product5["Title"] = "Red Hulk Alter Ego Action Figure";
      product5["ProductCode"] = "WP0005";
      product5["ProductListPrice"] = 9.99;
      product5["ProductDescription"] = "A case of anabolic steroids with a most unfortunate outcome.";
      product5["ProductColor"] = "Red";
      product5["MinimumAge"] = 7;
      product5["MaximumAge"] = 12;
      product5["ProductImageUrl"] = GetFieldUrlValue("WP0005", "Red Hulk Alter Ego Action Figure");
      product5.Update();
      Console.WriteLine("  Adding Red Hulk Alter Ego Action Figure");
      clientContext.ExecuteQuery();

      ListItem product6 = listProducts.AddItem(new ListItemCreationInformation());
      product6["Title"] = "Godzilla Action Figure";
      product6["ProductCode"] = "WP0006";
      product6["ProductListPrice"] = 19.95;
      product6["ProductDescription"] = "The classic and adorable action figure from those old Japanese movies.";
      product6["ProductColor"] = "Green";
      product6["MinimumAge"] = 10;
      product6["MaximumAge"] = null;
      product6["ProductImageUrl"] = GetFieldUrlValue("WP0006", "Godzilla Action Figure");
      product6.Update();
      Console.WriteLine("  Adding Godzilla Action Figure");
      clientContext.ExecuteQuery();

      ListItem product7 = listProducts.AddItem(new ListItemCreationInformation());
      product7["Title"] = "Perry the Platypus Action Figure";
      product7["ProductCode"] = "WP0007";
      product7["ProductListPrice"] = 21.95;
      product7["ProductDescription"] = "A platypus who plays an overly intelligent detective sleuth on TV.";
      product7["ProductColor"] = new string[] { "Green", "Yellow" };
      product7["MinimumAge"] = null;
      product7["MaximumAge"] = null;
      product7["ProductImageUrl"] = GetFieldUrlValue("WP0007", "Perry the Platypus Action Figure");
      product7.Update();
      Console.WriteLine("  Adding Perry the Platypus Action Figure");
      clientContext.ExecuteQuery();

      ListItem product8 = listProducts.AddItem(new ListItemCreationInformation());
      product8["Title"] = "Green Angry Bird Action Figure";
      product8["ProductCode"] = "WP0008";
      product8["ProductListPrice"] = 4.95;
      product8["ProductDescription"] = "A funny looking green bird that really hates pigs.";
      product8["ProductColor"] = "Green";
      product8["MinimumAge"] = 5;
      product8["MaximumAge"] = 10;
      product8["ProductImageUrl"] = GetFieldUrlValue("WP0008", "Green Angry Bird Action Figure");
      product8.Update();
      Console.WriteLine("  Adding Green Angry Bird Action Figure");
      clientContext.ExecuteQuery();

      ListItem product9 = listProducts.AddItem(new ListItemCreationInformation());
      product9["Title"] = "Red Angry Bird Action Figure";
      product9["ProductCode"] = "WP0009";
      product9["ProductListPrice"] = 14.95;
      product9["ProductDescription"] = "A funny looking red bird that also hates pigs.";
      product9["ProductColor"] = "Red";
      product9["MinimumAge"] = 5;
      product9["MaximumAge"] = 10;
      product9["ProductImageUrl"] = GetFieldUrlValue("WP0009", "Red Angry Bird Action Figure");
      product9.Update();
      Console.WriteLine("  Adding Red Angry Bird Action Figure");
      clientContext.ExecuteQuery();

      ListItem product10 = listProducts.AddItem(new ListItemCreationInformation());
      product10["Title"] = "Phineas and Ferb Action Figure Set";
      product10["ProductCode"] = "WP0010";
      product10["ProductListPrice"] = 19.95;
      product10["ProductDescription"] = "The dynamic duo of the younger generation.";
      product10["ProductColor"] = new string[] { "Green", "Red" };
      product10["MinimumAge"] = 5;
      product10["MaximumAge"] = 51;
      product10["ProductImageUrl"] = GetFieldUrlValue("WP0010", "Phineas and Ferb Action Figure Set");
      product10.Update();
      Console.WriteLine("  Adding Phineas and Ferb Action Figure Set.");
      clientContext.ExecuteQuery();

      ListItem product11 = listProducts.AddItem(new ListItemCreationInformation());
      product11["Title"] = "Black Power Ranger Action Figure";
      product11["ProductCode"] = "WP0011";
      product11["ProductListPrice"] = 7.50;
      product11["ProductDescription"] = "A particularly violent action figure for violent children.";
      product11["ProductColor"] = new string[] { "Black", "White" };
      product11["MinimumAge"] = 8;
      product11["MaximumAge"] = 12;
      product11["ProductImageUrl"] = GetFieldUrlValue("WP0011", "Black Power Ranger Action Figure");
      product11.Update();
      Console.WriteLine("  Adding Black Power Ranger Action Figure");
      clientContext.ExecuteQuery();

      ListItem product12 = listProducts.AddItem(new ListItemCreationInformation());
      product12["Title"] = "Woody Action Figure";
      product12["ProductCode"] = "WP0012";
      product12["ProductListPrice"] = 9.95;
      product12["ProductDescription"] = "The lovable, soft-spoken cowboy from Toy Story.";
      product12["ProductColor"] = new string[] { "Blue", "Yellow" };
      product12["MinimumAge"] = null;
      product12["MaximumAge"] = 12;
      product12["ProductImageUrl"] = GetFieldUrlValue("WP0012", "Woody Action Figure");
      product12.Update();
      Console.WriteLine("  Adding Woody Action Figure");
      clientContext.ExecuteQuery();

      ListItem product13 = listProducts.AddItem(new ListItemCreationInformation());
      product13["Title"] = "Spiderman Action Figure";
      product13["ProductCode"] = "WP0013";
      product13["ProductListPrice"] = 12.95;
      product13["ProductDescription"] = "The classic superhero who is quite the swinger.";
      product13["ProductColor"] = new string[] { "Red", "Blue" };
      product13["MinimumAge"] = 8;
      product13["MaximumAge"] = 12;
      product13["ProductImageUrl"] = GetFieldUrlValue("WP0013", "Spiderman Action Figure");
      product13.Update();
      Console.WriteLine("  Adding Spiderman Action Figure");
      clientContext.ExecuteQuery();

      ListItem product14 = listProducts.AddItem(new ListItemCreationInformation());
      product14["Title"] = "Twitter Follower Action Figure";
      product14["ProductCode"] = "WP0014";
      product14["ProductListPrice"] = 1.00;
      product14["ProductDescription"] = "An inexpensive action figure you can never have too many of.";
      product14["ProductColor"] = new string[] { "Yellow", "Blue" };
      product14["MinimumAge"] = 12;
      product14["MaximumAge"] = null;
      product14["ProductImageUrl"] = GetFieldUrlValue("WP0014", "Twitter Follower Action Figure");
      product14.Update();
      Console.WriteLine("  Adding Twitter Follower Action Figure");
      clientContext.ExecuteQuery();

      ListItem product15 = listProducts.AddItem(new ListItemCreationInformation());
      product15["Title"] = "Crayloa Crayon Set";
      product15["ProductCode"] = "WP0015";
      product15["ProductListPrice"] = 2.49;
      product15["ProductDescription"] = "A very fun set of crayons in every color.";
      product15["ProductColor"] = new string[] { "Blue", "Red", "Green", "Yellow" };
      product15["MinimumAge"] = 10;
      product15["MaximumAge"] = null;
      product15["ProductImageUrl"] = GetFieldUrlValue("WP0015", "Crayloa Crayon Set");
      product15.Update();
      Console.WriteLine("  Adding Crayloa Crayon Set");
      clientContext.ExecuteQuery();

      ListItem product16 = listProducts.AddItem(new ListItemCreationInformation());
      product16["Title"] = "Sponge Bob Coloring Book";
      product16["ProductCode"] = "WP0016";
      product16["ProductListPrice"] = 2.95;
      product16["ProductDescription"] = "An action figure of America's most recognizable celebrity.";
      product16["ProductColor"] = "Yellow";
      product16["MinimumAge"] = 7;
      product16["MaximumAge"] = 12;
      product16["ProductImageUrl"] = GetFieldUrlValue("WP0016", "Sponge Bob Coloring Book");
      product16.Update();
      Console.WriteLine("  Adding Sponge Bob Coloring Book");
      clientContext.ExecuteQuery();

      ListItem product17 = listProducts.AddItem(new ListItemCreationInformation());
      product17["Title"] = "Easel with Supply Trays";
      product17["ProductCode"] = "WP0017";
      product17["ProductListPrice"] = 49.95;
      product17["ProductDescription"] = "A serious easel for serious young artists.";
      product17["ProductColor"] = "White";
      product17["MinimumAge"] = 12;
      product17["MaximumAge"] = null;
      product17["ProductImageUrl"] = GetFieldUrlValue("WP0017", "Easel with Supply Trays");
      product17.Update();
      Console.WriteLine("  Adding Easel with Supply Trays");
      clientContext.ExecuteQuery();

      ListItem product18 = listProducts.AddItem(new ListItemCreationInformation());
      product18["Title"] = "Crate o' Crayons";
      product18["ProductCode"] = "WP0018";
      product18["ProductListPrice"] = 14.95;
      product18["ProductDescription"] = "More crayons that you can shake a stick at.";
      product18["ProductColor"] = new string[] { "Blue", "Red", "Green", "Yellow" };
      product18["MinimumAge"] = 7;
      product18["MaximumAge"] = 12;
      product18["ProductImageUrl"] = GetFieldUrlValue("WP0018", "Crate o' Crayons");
      product18.Update();
      Console.WriteLine("  Adding Crate o' Crayons");
      clientContext.ExecuteQuery();

      ListItem product19 = listProducts.AddItem(new ListItemCreationInformation());
      product19["Title"] = "Etch A Sketch";
      product19["ProductCode"] = "WP0019";
      product19["ProductListPrice"] = 12.95;
      product19["ProductDescription"] = "A strategic planning tool for the Romney campaign.";
      product19["ProductColor"] = "Red";
      product19["MinimumAge"] = 7;
      product19["MaximumAge"] = null;
      product19["ProductImageUrl"] = GetFieldUrlValue("WP0019", "Etch A Sketch");
  
      product19.Update();
      Console.WriteLine("  Adding Etch A Sketch");
      clientContext.ExecuteQuery();

      ListItem product20 = listProducts.AddItem(new ListItemCreationInformation());
      product20["Title"] = "Green Hornet";
      product20["ProductCode"] = "WP0020";
      product20["ProductListPrice"] = 24.95;
      product20["ProductDescription"] = "A fast car for crusin' the strip at night.";
      product20["ProductColor"] = "Green";
      product20["MinimumAge"] = 10;
      product20["MaximumAge"] = null;
      product20["ProductImageUrl"] = GetFieldUrlValue("WP0032", "Green Hornet");
      product20.Update();
      Console.WriteLine("  Adding Green Hornet");
      clientContext.ExecuteQuery();

      ListItem product21 = listProducts.AddItem(new ListItemCreationInformation());
      product21["Title"] = "Red Wacky Stud Bumper";
      product21["ProductCode"] = "WP0021";
      product21["ProductListPrice"] = 24.95;
      product21["ProductDescription"] = "A great little vehicle for off road fun.";
      product21["ProductColor"] = "Red";
      product21["MinimumAge"] = 10;
      product21["MaximumAge"] = null;
      product21["ProductImageUrl"] = GetFieldUrlValue("WP0021", "Red Wacky Stud Bumper");
      product21.Update();
      Console.WriteLine("  Adding Red Wacky Stud Bumper");
      clientContext.ExecuteQuery();

      ListItem product22 = listProducts.AddItem(new ListItemCreationInformation());
      product22["Title"] = "Red Stomper Bully";
      product22["ProductCode"] = "WP0022";
      product22["ProductListPrice"] = 29.95;
      product22["ProductDescription"] = "A great toy that can crush and destroy all your other toys.";
      product22["ProductColor"] = "Red";
      product22["MinimumAge"] = 10;
      product22["MaximumAge"] = null;
      product22["ProductImageUrl"] = GetFieldUrlValue("WP0022", "Red Stomper Bully");
      product22.Update();
      Console.WriteLine("  Adding Red Stomper Bully");
      clientContext.ExecuteQuery();

      ListItem product23 = listProducts.AddItem(new ListItemCreationInformation());
      product23["Title"] = "Green Stomper Bully";
      product23["ProductCode"] = "WP0023";
      product23["ProductListPrice"] = 24.95;
      product23["ProductDescription"] = "A green alternative to crush and destroy the Red Stomper Bully.";
      product23["ProductColor"] = "Green";
      product23["MinimumAge"] = 10;
      product23["MaximumAge"] = null;
      product23["ProductImageUrl"] = GetFieldUrlValue("WP0023", "Green Stomper Bully");
      product23.Update();
      Console.WriteLine("  Adding Green Stomper Bully");
      clientContext.ExecuteQuery();

      ListItem product24 = listProducts.AddItem(new ListItemCreationInformation());
      product24["Title"] = "Indy Race Car";
      product24["ProductCode"] = "WP0024";
      product24["ProductListPrice"] = 19.95;
      product24["ProductDescription"] = "The fastest remote control race car on the market today.";
      product24["ProductColor"] = "Black";
      product24["MinimumAge"] = 10;
      product24["MaximumAge"] = null;
      product24["ProductImageUrl"] = GetFieldUrlValue("WP0024", "Indy Race Car");
      product24.Update();
      Console.WriteLine("  Adding Indy Race Car");
      clientContext.ExecuteQuery();

      ListItem product25 = listProducts.AddItem(new ListItemCreationInformation());
      product25["Title"] = "Turbo-boost Speedboat";
      product25["ProductCode"] = "WP0025";
      product25["ProductListPrice"] = 32.95;
      product25["ProductDescription"] = "The preferred water vehicle of gun runners and drug kingpins.";
      product25["ProductColor"] = "Red";
      product25["MinimumAge"] = 21;
      product25["MaximumAge"] = null;
      product25["ProductImageUrl"] = GetFieldUrlValue("WP0025", "Turbo-boost Speedboat");
      product25.Update();
      Console.WriteLine("  Adding Turbo-boost Speedboat");
      clientContext.ExecuteQuery();

      ListItem product26 = listProducts.AddItem(new ListItemCreationInformation());
      product26["Title"] = "Sandpiper Prop Plane";
      product26["ProductCode"] = "WP0026";
      product26["ProductListPrice"] = 24.95;
      product26["ProductDescription"] = "A simple RC prop plane for younger pilots.";
      product26["ProductColor"] = "White";
      product26["MinimumAge"] = 15;
      product26["MaximumAge"] = null;
      product26["ProductImageUrl"] = GetFieldUrlValue("WP0026", "Sandpiper Prop Plane");
      product26.Update();
      Console.WriteLine("  Adding Sandpiper Prop Plane");
      clientContext.ExecuteQuery();

      ListItem product27 = listProducts.AddItem(new ListItemCreationInformation());
      product27["Title"] = "Flying Badger";
      product27["ProductCode"] = "WP0027";
      product27["ProductListPrice"] = 27.95;
      product27["ProductDescription"] = "A tough fighter plane to root out evil anywhere it lives.";
      product27["ProductColor"] = "Blue";
      product27["MinimumAge"] = 15;
      product27["MaximumAge"] = null;
      product27["ProductImageUrl"] = GetFieldUrlValue("WP0027", "Flying Badger");
      product27.Update();
      Console.WriteLine("  Adding Flying Badger");
      clientContext.ExecuteQuery();

      ListItem product28 = listProducts.AddItem(new ListItemCreationInformation());
      product28["Title"] = "Red Barron von Richthofen";
      product28["ProductCode"] = "WP0028";
      product28["ProductListPrice"] = 32.95;
      product28["ProductDescription"] = "A classic RC plane to hunt down and terminate Snoopy.";
      product28["ProductColor"] = "Red";
      product28["MinimumAge"] = 15;
      product28["MaximumAge"] = null;
      product28["ProductImageUrl"] = GetFieldUrlValue("WP0028", "Red Barron von Richthofen");
      product28.Update();
      Console.WriteLine("  Adding Red Barron von Richthofen");
      clientContext.ExecuteQuery();

      ListItem product29 = listProducts.AddItem(new ListItemCreationInformation());
      product29["Title"] = "Flying Squirrel";
      product29["ProductCode"] = "WP0029";
      product29["ProductListPrice"] = 69.95;
      product29["ProductDescription"] = "A stealthy remote control plane that flies on the down-low and under the radar.";
      product29["ProductColor"] = "Grey";
      product29["MinimumAge"] = 18;
      product29["MaximumAge"] = null;
      product29["ProductImageUrl"] = GetFieldUrlValue("WP0029", "Flying Squirrel");
      product29.Update();
      Console.WriteLine("  Adding Flying Squirrel");
      clientContext.ExecuteQuery();

      ListItem product30 = listProducts.AddItem(new ListItemCreationInformation());
      product30["Title"] = "FOX News Chopper";
      product30["ProductCode"] = "WP0030";
      product30["ProductListPrice"] = 29.95;
      product30["ProductDescription"] = "A new chopper which can generate new events on demand.";
      product30["ProductColor"] = "Blue";
      product30["MinimumAge"] = 18;
      product30["MaximumAge"] = null;
      product30["ProductImageUrl"] = GetFieldUrlValue("WP0030", "FOX News Chopper");
      product30.Update();
      Console.WriteLine("  Adding FOX News Chopper");
      clientContext.ExecuteQuery();

      ListItem product31 = listProducts.AddItem(new ListItemCreationInformation());
      product31["Title"] = "Seal Team 6 Helicopter";
      product31["ProductCode"] = "WP0031";
      product31["ProductListPrice"] = 59.95;
      product31["ProductDescription"] = "A serious helicopter that can open up a can of whoop-ass when required.";
      product31["ProductColor"] = "Green";
      product31["MinimumAge"] = 18;
      product31["MaximumAge"] = null;
      product31["ProductImageUrl"] = GetFieldUrlValue("WP0031", "Seal Team 6 Helicopter");
      product31.Update();
      Console.WriteLine("  Adding Seal Team 6 Helicopter");
      clientContext.ExecuteQuery();

      ListItem product32 = listProducts.AddItem(new ListItemCreationInformation());
      product32["Title"] = "Personal Commuter Chopper";
      product32["ProductCode"] = "WP0032";
      product32["ProductListPrice"] = 99.95;
      product32["ProductDescription"] = "A partially-test remote control device that can actually carry real people.";
      product32["ProductColor"] = "Red";
      product32["MinimumAge"] = 18;
      product32["MaximumAge"] = null;
      product32["ProductImageUrl"] = GetFieldUrlValue("WP0032", "Personal Commuter Chopper");
      product32.Update();
      Console.WriteLine("  Adding Personal Commuter Chopper");
      clientContext.ExecuteQuery();

      Console.WriteLine();
      Console.WriteLine("  Loading of product items has completed");
      Console.WriteLine();
    }

    private static FieldUrlValue GetFieldUrlValue(string ProductCode, string ImageDescription) {
      FieldUrlValue urlValue = new FieldUrlValue();
      urlValue.Url = listProductImagesUrl + "/" + ProductCode + ".jpg";
      urlValue.Description = ImageDescription;
      return urlValue;
    }

    #endregion

    #region "Customers List"

    static List listCustomers;

    public static void CreateCustomersList(int CustomerCount, int BatchSize) {

      Console.WriteLine("Creating customers list...");


      DeleteList("Customers");

      ListCreationInformation listInformationCustomers = new ListCreationInformation();
      listInformationCustomers.Title = "Customers";
      listInformationCustomers.Url = "Lists/Customers";
      listInformationCustomers.QuickLaunchOption = QuickLaunchOptions.On;
      listInformationCustomers.TemplateType = (int)ListTemplateType.Contacts;
      listCustomers = site.Lists.Add(listInformationCustomers);
      listCustomers.OnQuickLaunch = true;
      listCustomers.Update();
      clientContext.ExecuteQuery();

      PopulateCustomersList(CustomerCount, BatchSize);
    }

    static void PopulateCustomersList(int CustomerCount, int BatchSize) {

      Console.WriteLine();
      Console.WriteLine("Adding sample Customers list...");
      Console.WriteLine();

      int customerCounter = 0;
      int batchCounter = 0;
      int batchStart = 1;

      var customers = RandomCustomerGenerator.GetCustomerList(CustomerCount);

      foreach(var customer in customers) {
        // increment counters
        customerCounter += 1;
        batchCounter += 1;
        // add new customer item
        ListItem newCustomer = listCustomers.AddItem(new ListItemCreationInformation());
        newCustomer["Title"] = customer.LastName;
        newCustomer["FirstName"] = customer.FirstName;
        newCustomer["Company"] = customer.Company;
        newCustomer["HomePhone"] = customer.HomePhone;
        newCustomer["WorkPhone"] = customer.WorkPhone;
        newCustomer["Email"] = customer.EmailAddress;
        newCustomer["WorkAddress"] = customer.Address;
        newCustomer["WorkCity"] = customer.City;
        newCustomer["WorkState"] = customer.State;
        newCustomer["WorkZip"] = customer.ZipCode;
        newCustomer.Update();
        if(batchCounter >= BatchSize) {
          Console.WriteLine("Adding customers " + batchStart.ToString() + " to " + customerCounter + "...");
          clientContext.ExecuteQuery();
          batchCounter= 0;
          batchStart = customerCounter + 1;
        }        
      }
      clientContext.ExecuteQuery();

      Console.WriteLine();
      Console.WriteLine("  Loading of customer data has completed");
      Console.WriteLine();
    }

    #endregion

    #region "Expenses List"

    public static void CreateExpensesLists() {
      DeleteExpenseListTypes();
      CreateExpenseSiteColumns();
      CreateExpenseContentTypes();
      CreateExpensesList();
      CreateExpenseBudgetsList();
    }

    static FieldChoice fldExpenseCategory;
    static FieldDateTime fldExpenseDate;
    static FieldCurrency fldExpenseAmount;


    static FieldText fldExpenseBudgetYear;
    static FieldText fldExpenseBudgetQuarter;
    static FieldCurrency fldExpenseBudgetAmount;

    static ContentType ctypeExpense;
    static ContentType ctypeExpenseBudgetItem;

    static List listExpenses;
    static List listExpenseBudgets;

    static void DeleteExpenseListTypes() {
      DeleteList("Expenses");
      DeleteList("Expense Budgets");
      DeleteContentType("Expense Item");
      DeleteContentType("Expense Budget Item");
    }

    class ExpenseCategory {
      public const string OfficeSupplies = "Office Supplies";
      public const string Marketing = "Marketing";
      public const string Operations = "Operations";
      public const string ResearchAndDevelopment = "Research & Development";
      public static string[] GetAll() {
        string[] AllCategories = { OfficeSupplies, Marketing, Operations, ResearchAndDevelopment };
        return AllCategories;
      }
    }

    static void CreateExpenseSiteColumns() {

      fldExpenseCategory = clientContext.CastTo<FieldChoice>(CreateSiteColumn("ExpenseCategory", "Expense Category", "Choice"));
      string[] choicesExpenseCategory = ExpenseCategory.GetAll();
      fldExpenseCategory.Choices = choicesExpenseCategory;
      fldExpenseCategory.Update();
      clientContext.ExecuteQuery();


      fldExpenseDate = clientContext.CastTo<FieldDateTime>(CreateSiteColumn("ExpenseDate", "Expense Date", "DateTime")); ;
      fldExpenseDate.DisplayFormat = DateTimeFieldFormatType.DateOnly;
      fldExpenseDate.Update();

      fldExpenseAmount = clientContext.CastTo<FieldCurrency>(CreateSiteColumn("ExpenseAmount", "Expense Amount", "Currency"));
      fldExpenseAmount.MinimumValue = 0;

      fldExpenseBudgetYear = clientContext.CastTo<FieldText>(CreateSiteColumn("ExpenseBudgetYear", "Budget Year", "Text"));

      fldExpenseBudgetQuarter = clientContext.CastTo<FieldText>(CreateSiteColumn("ExpenseBudgetQuarter", "Budget Quarter", "Text"));
      fldExpenseBudgetQuarter.Update();

      fldExpenseBudgetAmount = clientContext.CastTo<FieldCurrency>(CreateSiteColumn("ExpenseBudgetAmount", "Budget Amount", "Currency"));

      clientContext.ExecuteQuery();
    }

    static void CreateExpenseContentTypes() {

      ctypeExpense = CreateContentType("Expense Item", "0x01");
      ctypeExpense.Update(true);
      clientContext.Load(ctypeExpense.FieldLinks);
      clientContext.ExecuteQuery();

      FieldLinkCreationInformation fldLinkExpenseCategory = new FieldLinkCreationInformation();
      fldLinkExpenseCategory.Field = fldExpenseCategory;
      ctypeExpense.FieldLinks.Add(fldLinkExpenseCategory);
      ctypeExpense.Update(true);

      // add site columns
      FieldLinkCreationInformation fldLinkExpenseDate = new FieldLinkCreationInformation();
      fldLinkExpenseDate.Field = fldExpenseDate;
      ctypeExpense.FieldLinks.Add(fldLinkExpenseDate);
      ctypeExpense.Update(true);

      // add site columns
      FieldLinkCreationInformation fldLinkExpenseAmount = new FieldLinkCreationInformation();
      fldLinkExpenseAmount.Field = fldExpenseAmount;
      ctypeExpense.FieldLinks.Add(fldLinkExpenseAmount);
      ctypeExpense.Update(true);

      clientContext.ExecuteQuery();

      ctypeExpenseBudgetItem = CreateContentType("Expense Budget Item", "0x01");
      ctypeExpenseBudgetItem.Update(true);
      clientContext.Load(ctypeExpenseBudgetItem.FieldLinks);
      clientContext.ExecuteQuery();

      FieldLinkCreationInformation fldLinkExpenseBudgetCategory = new FieldLinkCreationInformation();
      fldLinkExpenseBudgetCategory.Field = fldExpenseCategory;
      ctypeExpenseBudgetItem.FieldLinks.Add(fldLinkExpenseBudgetCategory);
      ctypeExpenseBudgetItem.Update(true);

      FieldLinkCreationInformation fldLinkExpenseBudgetYear = new FieldLinkCreationInformation();
      fldLinkExpenseBudgetYear.Field = fldExpenseBudgetYear;
      ctypeExpenseBudgetItem.FieldLinks.Add(fldLinkExpenseBudgetYear);
      ctypeExpenseBudgetItem.Update(true);

      FieldLinkCreationInformation fldLinkExpenseBudgetQuarter = new FieldLinkCreationInformation();
      fldLinkExpenseBudgetQuarter.Field = fldExpenseBudgetQuarter;
      ctypeExpenseBudgetItem.FieldLinks.Add(fldLinkExpenseBudgetQuarter);
      ctypeExpenseBudgetItem.Update(true);

      FieldLinkCreationInformation fldLinkExpenseBudgetAmount = new FieldLinkCreationInformation();
      fldLinkExpenseBudgetAmount.Field = fldExpenseBudgetAmount;
      ctypeExpenseBudgetItem.FieldLinks.Add(fldLinkExpenseBudgetAmount);
      ctypeExpenseBudgetItem.Update(true);

      clientContext.ExecuteQuery();

    }

    static void CreateExpensesList() {

      string listTitle = "Expenses";
      string listUrl = "Lists/Expenses";

      // delete document library if it already exists
      ExceptionHandlingScope scope = new ExceptionHandlingScope(clientContext);
      using (scope.StartScope()) {
        using (scope.StartTry()) {
          site.Lists.GetByTitle(listTitle).DeleteObject();
        }
        using (scope.StartCatch()) { }
      }

      ListCreationInformation lci = new ListCreationInformation();
      lci.Title = listTitle;
      lci.Url = listUrl;
      lci.TemplateType = (int)ListTemplateType.GenericList;
      listExpenses = site.Lists.Add(lci);
      listExpenses.OnQuickLaunch = true;
      listExpenses.EnableFolderCreation = false;
      listExpenses.Update();


      // attach JSLink script to default view for client-side rendering
      //listExpenses.DefaultView.JSLink = AppRootFolderRelativeUrl + "scripts/CustomersListCSR.js";
      listExpenses.DefaultView.Update();
      listExpenses.Update();
      clientContext.Load(listExpenses);
      clientContext.Load(listExpenses.Fields);
      var titleField = listExpenses.Fields.GetByInternalNameOrTitle("Title");
      titleField.Title = "Expense Description";
      titleField.Update();
      clientContext.ExecuteQuery();

      listExpenses.ContentTypesEnabled = true;
      listExpenses.ContentTypes.AddExistingContentType(ctypeExpense);
      listExpenses.Update();
      clientContext.Load(listExpenses.ContentTypes);
      clientContext.ExecuteQuery();

      ContentType existing = listExpenses.ContentTypes[0];
      existing.DeleteObject();
      clientContext.ExecuteQuery();

      View viewProducts = listExpenses.DefaultView;

      viewProducts.ViewFields.Add("ExpenseCategory");
      viewProducts.ViewFields.Add("ExpenseDate");
      viewProducts.ViewFields.Add("ExpenseAmount");
      viewProducts.Update();

      clientContext.ExecuteQuery();

      PopulateExpensesList();

    }

    static void CreateExpenseBudgetsList() {

      string listTitle = "Expense Budgets";
      string listUrl = "Lists/ExpenseBudgets";

      // delete document library if it already exists
      ExceptionHandlingScope scope = new ExceptionHandlingScope(clientContext);
      using (scope.StartScope()) {
        using (scope.StartTry()) {
          site.Lists.GetByTitle(listTitle).DeleteObject();
        }
        using (scope.StartCatch()) { }
      }

      ListCreationInformation lci = new ListCreationInformation();
      lci.Title = listTitle;
      lci.Url = listUrl;
      lci.TemplateType = (int)ListTemplateType.GenericList;
      listExpenseBudgets = site.Lists.Add(lci);
      listExpenseBudgets.OnQuickLaunch = true;
      listExpenseBudgets.EnableFolderCreation = false;
      listExpenseBudgets.Update();

      listExpenseBudgets.DefaultView.Update();
      listExpenseBudgets.Update();
      clientContext.Load(listExpenseBudgets);
      clientContext.Load(listExpenseBudgets.Fields);
      var titleField = listExpenseBudgets.Fields.GetByInternalNameOrTitle("Title");
      titleField.Title = "Expense Budget";
      titleField.Update();
      clientContext.ExecuteQuery();

      listExpenseBudgets.ContentTypesEnabled = true;
      listExpenseBudgets.ContentTypes.AddExistingContentType(ctypeExpenseBudgetItem);
      listExpenseBudgets.Update();
      clientContext.Load(listExpenseBudgets.ContentTypes);
      clientContext.ExecuteQuery();

      ContentType existing = listExpenseBudgets.ContentTypes[0];
      existing.DeleteObject();
      clientContext.ExecuteQuery();

      View viewProducts = listExpenseBudgets.DefaultView;

      viewProducts.ViewFields.Add("ExpenseCategory");
      viewProducts.ViewFields.Add("ExpenseBudgetYear");
      viewProducts.ViewFields.Add("ExpenseBudgetQuarter");
      viewProducts.ViewFields.Add("ExpenseBudgetAmount");
      viewProducts.Update();

      clientContext.ExecuteQuery();

      PopulateExpenseBudgetsList();

    }

    static void AddExpense(string Description, string Category, DateTime Date, decimal Amount) {

      ListItem newItem = listExpenses.AddItem(new ListItemCreationInformation());
      newItem["Title"] = Description;
      newItem["ExpenseCategory"] = Category;
      newItem["ExpenseDate"] = Date;
      newItem["ExpenseAmount"] = Amount;

      newItem.Update();
      clientContext.ExecuteQuery();

      Console.Write(".");
    }

    static void PopulateExpensesList() {

      Console.Write("Adding expenses");

      // January 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 1, 3), 133.44m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 1, 3), 328.40m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 1, 5), 824.90m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 1, 8), 89.40m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 1, 18), 23.90m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 1, 21), 478.33m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 1, 21), 20.00m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 1, 24), 12.50m);
      AddExpense("Toy Stress Tester", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 1, 28), 2400.00m);
      AddExpense("Office Depot supply run", ExpenseCategory.OfficeSupplies, new DateTime(2016, 1, 29), 184.30m);

      // Feb 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 2, 1), 138.02m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 2, 1), 297.47m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 2, 1), 789.77m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 8.95m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 74.55m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 45.67m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 32.34m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 20m);
      AddExpense("Toy Stress Tester", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 2, 1), 2400m);
      AddExpense("Office Depot supply run", ExpenseCategory.OfficeSupplies, new DateTime(2016, 2, 1), 196.44m);
      AddExpense("TV Ads - East Coast", ExpenseCategory.Marketing, new DateTime(2016, 2, 1), 2800m);
      AddExpense("TV Ads - West Coast", ExpenseCategory.Marketing, new DateTime(2016, 2, 1), 2400m);

      // March 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 3, 1), 142.99m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 3, 1), 304.21m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 3, 1), 804.33m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 44.23m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 3, 1), 500m);
      AddExpense("Printer Paper", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 48.20m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 20m);
      AddExpense("Toner Cartridges for Printer", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 220.34m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 8.95m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 3, 1), 12.30m);

      // April 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 4, 1), 138.34m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 4, 1), 344.32m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 4, 1), 812.90m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 4, 1), 32.45m);
      AddExpense("Toy Stress Tester", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 4, 1), 2400m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 4, 1), 500m);
      AddExpense("Print Ad in People Magazine", ExpenseCategory.Marketing, new DateTime(2016, 4, 1), 1200m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 4, 1), 34.20m);
      AddExpense("Toner Cartridges for Printer", ExpenseCategory.OfficeSupplies, new DateTime(2016, 4, 1), 127.88m);


      // May 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 5, 1), 152.55m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 5, 1), 320.45m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 5, 1), 783.44m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 5, 1), 23.90m);
      AddExpense("Toner Cartridges for Printer", ExpenseCategory.OfficeSupplies, new DateTime(2016, 5, 1), 240.50m);
      AddExpense("Printer Paper", ExpenseCategory.OfficeSupplies, new DateTime(2016, 5, 1), 22.32m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 5, 1), 20m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 5, 1), 8.95m);


      // June 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 6, 1), 138.44m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 6, 1), 332.78m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 6, 1), 802.44m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 6, 1), 34.22m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 6, 1), 8.95m);
      AddExpense("Print Ad in People Magazine", ExpenseCategory.Marketing, new DateTime(2016, 6, 1), 1200m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 6, 1), 24.10m);
      AddExpense("Toner Cartridges for Printer", ExpenseCategory.OfficeSupplies, new DateTime(2016, 6, 1), 132.20m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 6, 1), 8.95m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 6, 1), 500m);

      // July 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 7, 1), 135.22m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 7, 1), 333.11m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 7, 1), 798.25m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 7, 1), 8.95m);
      AddExpense("Office Depot supply run", ExpenseCategory.OfficeSupplies, new DateTime(2016, 7, 1), 212.41m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 7, 1), 46.78m);
      AddExpense("Particle Accelerator", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 7, 1), 4800m);

      // August 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 8, 1), 142.20m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 8, 1), 345.80m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 8, 1), 814.87m);
      AddExpense("TV Ads - Southeast", ExpenseCategory.Marketing, new DateTime(2016, 8, 1), 2800m);
      AddExpense("Toy Stress Tester", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 8, 1), 2400m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 8, 1), 8.95m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 8, 1), 500m);
      AddExpense("Server computer", ExpenseCategory.Operations, new DateTime(2016, 8, 1), 2500m);
      AddExpense("Office chairs", ExpenseCategory.OfficeSupplies, new DateTime(2016, 8, 1), 890.10m);


      // September 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 9, 1), 136.10m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 9, 1), 326.01m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 9, 1), 802.90m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 42.34m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 8.95m);
      AddExpense("Printer Paper", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 86.10m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 20m);
      AddExpense("Toner Cartridges for Printer", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 190.50m);
      AddExpense("Paper clips", ExpenseCategory.OfficeSupplies, new DateTime(2016, 9, 1), 8.95m);
      AddExpense("Server computer", ExpenseCategory.Operations, new DateTime(2016, 9, 1), 3200m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 9, 1), 500m);


      // October 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 10, 1), 141.33m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 10, 1), 322.55m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 10, 1), 832.50m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 10, 1), 35.34m);
      AddExpense("TV Ads - Southeast", ExpenseCategory.Marketing, new DateTime(2016, 10, 1), 4800m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 10, 1), 20m);
      AddExpense("Office Depot supply run", ExpenseCategory.OfficeSupplies, new DateTime(2016, 10, 1), 107.33m);
      AddExpense("Server computer", ExpenseCategory.Operations, new DateTime(2016, 10, 1), 2800m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 10, 1), 8.95m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 10, 1), 30.66m);
      AddExpense("Slide rule", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 10, 1), 48.50m);


      // November 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 11, 1), 140.10m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 11, 1), 321.98m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 11, 1), 842.90m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 11, 1), 42.11m);
      AddExpense("TV Ads - West Coast", ExpenseCategory.Marketing, new DateTime(2016, 11, 1), 4800m);
      AddExpense("File cabinet", ExpenseCategory.OfficeSupplies, new DateTime(2016, 11, 1), 120m);
      AddExpense("Printer Paper", ExpenseCategory.OfficeSupplies, new DateTime(2016, 11, 1), 220.34m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 11, 1), 500m);
      AddExpense("Postage Stamps", ExpenseCategory.OfficeSupplies, new DateTime(2016, 11, 1), 20m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 11, 1), 28.35m);

      // December 2016
      AddExpense("Water Bill", ExpenseCategory.Operations, new DateTime(2016, 12, 1), 326.48m);
      AddExpense("Verizon - Telephone Expenses", ExpenseCategory.Operations, new DateTime(2016, 12, 1), 345.32m);
      AddExpense("Electricity Bill", ExpenseCategory.Operations, new DateTime(2016, 12, 1), 840.66m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 8.95m);
      AddExpense("Printer Paper", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 34.20m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 12, 1), 500m);
      AddExpense("Cleaning Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 144.50m);
      AddExpense("Particle Accelerator", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 12, 1), 1200m);
      AddExpense("Pencils", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 8.95m);
      AddExpense("TV Ads - Southeast", ExpenseCategory.Marketing, new DateTime(2016, 12, 1), 1200m);
      AddExpense("Science Calculator", ExpenseCategory.ResearchAndDevelopment, new DateTime(2016, 12, 1), 120m);
      AddExpense("TV Ads - East Coast", ExpenseCategory.Marketing, new DateTime(2016, 12, 1), 1800m);
      AddExpense("TV Ads - West Coast", ExpenseCategory.Marketing, new DateTime(2016, 12, 1), 900m);
      AddExpense("Coffee Supplies", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 45.33m);
      AddExpense("Google Ad Words", ExpenseCategory.Marketing, new DateTime(2016, 12, 1), 500m);
      AddExpense("Office chairs", ExpenseCategory.OfficeSupplies, new DateTime(2016, 12, 1), 780.32m);

      Console.WriteLine();
      Console.WriteLine();
    }

    static void AddExpenseBudget(string Category, string Year, string Quarter, decimal Amount) {

      ListItem newItem = listExpenseBudgets.AddItem(new ListItemCreationInformation());
      newItem["Title"] = Category + " for " + Quarter.ToString() + " of " + Year.ToString();
      newItem["ExpenseCategory"] = Category;
      newItem["ExpenseBudgetYear"] = Year;
      newItem["ExpenseBudgetQuarter"] = Quarter;
      newItem["ExpenseBudgetAmount"] = Amount;

      newItem.Update();
      clientContext.ExecuteQuery();

      Console.Write(".");
    }

    static void PopulateExpenseBudgetsList() {

      Console.Write("Adding expense budgets");

      AddExpenseBudget(ExpenseCategory.OfficeSupplies, "2016", "Q1", 1000m);
      AddExpenseBudget(ExpenseCategory.Marketing, "2016", "Q1", 7500m);
      AddExpenseBudget(ExpenseCategory.Operations, "2016", "Q1", 7000m);
      AddExpenseBudget(ExpenseCategory.ResearchAndDevelopment, "2016", "Q1", 5000m);

      AddExpenseBudget(ExpenseCategory.OfficeSupplies, "2016", "Q2", 1000m);
      AddExpenseBudget(ExpenseCategory.Marketing, "2016", "Q2", 7500m);
      AddExpenseBudget(ExpenseCategory.Operations, "2016", "Q2", 7000m);
      AddExpenseBudget(ExpenseCategory.ResearchAndDevelopment, "2016", "Q2", 5000m);

      AddExpenseBudget(ExpenseCategory.OfficeSupplies, "2016", "Q3", 1000m);
      AddExpenseBudget(ExpenseCategory.Marketing, "2016", "Q3", 10000m);
      AddExpenseBudget(ExpenseCategory.Operations, "2016", "Q3", 7000m);
      AddExpenseBudget(ExpenseCategory.ResearchAndDevelopment, "2016", "Q3", 5000m);

      AddExpenseBudget(ExpenseCategory.OfficeSupplies, "2016", "Q4", 1000m);
      AddExpenseBudget(ExpenseCategory.Marketing, "2016", "Q4", 10000m);
      AddExpenseBudget(ExpenseCategory.Operations, "2016", "Q4", 7000m);
      AddExpenseBudget(ExpenseCategory.ResearchAndDevelopment, "2016", "Q4", 5000m);


      Console.WriteLine();
      Console.WriteLine();
    }

    #endregion

  }
}
