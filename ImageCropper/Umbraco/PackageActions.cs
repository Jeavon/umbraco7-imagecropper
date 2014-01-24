using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropper.Umbraco
{
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.XPath;

    using global::Umbraco.Core.Logging;

    using umbraco;
    using umbraco.interfaces;

    using helper = umbraco.cms.businesslogic.packager.standardPackageActions.helper;

    public class AddAssemblyBinding : IPackageAction
    {
        public string Alias()
        {
            return "Umbundle.AddAssemblyBinding";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {

            // Set result default to false
            bool result = false;

            // Set insert node default true
            bool insertNode = true;

            // Set modified document default to false
            bool modified = false;

            // Get attribute values of xmlData
            string name, publicKeyToken, oldVersion, newVersion;
            if (!this.GetAttribute(xmlData, "name", out name) || !this.GetAttribute(xmlData, "publicKeyToken", out publicKeyToken) || !this.GetAttribute(xmlData, "oldVersion", out oldVersion) || !this.GetAttribute(xmlData, "newVersion", out newVersion))
            {
                return result;
            }

            string filename = HttpContext.Current.Server.MapPath("/web.config");
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filename);
            }
            catch (FileNotFoundException)
            {
            }

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("bindings", "urn:schemas-microsoft-com:asm.v1");

            XPathNavigator nav = document.CreateNavigator().SelectSingleNode("//bindings:assemblyBinding", nsmgr);
            if (nav == null)
            {
                throw new Exception("Invalid Configuration File");
            }

            // Look for existing nodes with same path like the new node
            if (nav.HasChildren)
            {
                // Look for existing nodeType nodes
                var node =
                    nav.SelectSingleNode(string.Format("./bindings:dependentAssembly/bindings:assemblyIdentity[@publicKeyToken = '{0}' and @name='{1}']", publicKeyToken, name), nsmgr);

                // If path already exists 
                if (node != null)
                {
                    if (node.MoveToNext())
                    {
                        if (node.MoveToAttribute("oldVersion", string.Empty))
                        {
                            node.SetValue(oldVersion);
                        }

                        if (node.MoveToParent())
                        {
                            if (node.MoveToAttribute("newVersion", string.Empty))
                            {
                                node.SetValue(newVersion);
                            }
                        }

                        // Cancel insert node operation
                        insertNode = false;

                        // Lets update versions instead
                        modified = true;
                    }
                    else
                    {
                        //Log error message
                        string message = "Error at AddAssemblyBinding package action: "
                             + "Updating \"" + name + "\" assembly binding failed.";
                        LogHelper.Warn(typeof(AddAssemblyBinding), message);
                    }
                }
            }

            // Check for insert flag
            if (insertNode)
            {
                var newNodeContent =
                    string.Format(
                        "<dependentAssembly><assemblyIdentity name=\"{0}\" publicKeyToken=\"{1}\" culture=\"neutral\" />"
                        + "<bindingRedirect oldVersion=\"{2}\" newVersion=\"{3}\" /></dependentAssembly>",
                        name,
                        publicKeyToken,
                        oldVersion,
                        newVersion);

                nav.AppendChild(newNodeContent);

                modified = true;

            }

            if (modified)
            {
                try
                {
                    document.Save(filename);

                    // No errors so the result is true
                    result = true;
                }
                catch (Exception e)
                {
                    // Log error message
                    string message = "Error at execute AddAssemblyBinding package action: " + e.Message;
                    LogHelper.Error(typeof(AddAssemblyBinding), message, e);
                }
            }
            return result;

        }

        public XmlNode SampleXml()
        {
            string str = "<Action runat=\"install\" undo=\"false\" alias=\"Umbundle.AddAssemblyBindin\">" +
                            "<dependentAssembly>" +
                                "<assemblyIdentity name=\"newone\" publicKeyToken=\"608967\" />" +
                                "<bindingRedirect oldVersion=\"1\" newwVersion=\"2\" />" +
                            "</dependentAssembly>" +
                         "</Action>";
            return helper.parseStringToXmlNode(str);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        /// <summary>
        /// Get a named attribute from xmlData root node
        /// </summary>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <param name="value">returns the attribute value from xmlData</param>
        /// <returns>True, when attribute value available</returns>
        private bool GetAttribute(XmlNode xmlData, string attribute, out string value)
        {
            //Set result default to false
            bool result = false;

            //Out params must be assigned
            value = String.Empty;

            //Search xml attribute
            XmlAttribute xmlAttribute = xmlData.Attributes[attribute];

            //When xml attribute exists
            if (xmlAttribute != null)
            {
                //Get xml attribute value
                value = xmlAttribute.Value;

                //Set result successful to true
                result = true;
            }
            else
            {
                //Log error message
                string message = "Error at AddAssemblyBinding package action: "
                     + "Attribute \"" + attribute + "\" not found.";
                LogHelper.Warn(typeof(AddAssemblyBinding), message);
            }
            return result;
        }

    }

    public class AddHttpModule : IPackageAction
    {
        //Set the web.config full path
        const string FULL_PATH = "~/web.config";

        #region IPackageAction Members

        /// <summary>
        /// This Alias must be unique and is used as an identifier that must match 
        /// the alias in the package action XML
        /// </summary>
        /// <returns>The Alias in string format</returns>
        public string Alias()
        {
            return "Umbundle.AddHttpModule";
        }

        /// <summary>
        /// Append the xmlData node to the web.config file
        /// </summary>
        /// <param name="packageName">Name of the package that we install</param>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <returns>True when succeeded</returns>
        public bool Execute(string packageName, XmlNode xmlData)
        {
            //Set result default to false
            bool result = false;

            //Get attribute values of xmlData
            string position, path, verb, type, validate, name, preCondition;
            position = getAttributeDefault(xmlData, "position", null);
            if (!getAttribute(xmlData, "type", out type)) return result;
            name = getAttributeDefault(xmlData, "name", null);

            //Create a new xml document
            XmlDocument document = new XmlDocument();

            //Keep current indentions format
            document.PreserveWhitespace = true;

            //Load the web.config file into the xml document
            document.Load(HttpContext.Current.Server.MapPath(FULL_PATH));

            //Set modified document default to false
            bool modified = false;

            #region IIS6

            //Select root node in the web.config file for insert new nodes
            XmlNode rootNode = document.SelectSingleNode("//configuration/system.web/httpModules");

            //Set insert node default true
            bool insertNode = true;

            //Check for rootNode exists
            if (rootNode != null)
            {
                //Look for existing nodes with same path like the new node
                if (rootNode.HasChildNodes)
                {
                    //Look for existing nodeType nodes
                    XmlNode node = rootNode.SelectSingleNode(
                        String.Format("add[@name = '{0}']", name));

                    //If path already exists 
                    if (node != null)
                    {
                        //Cancel insert node operation
                        insertNode = false;
                    }
                }
                //Check for insert flag
                if (insertNode)
                {
                    //Create new node with attributes
                    XmlNode newNode = document.CreateElement("add");
                    newNode.Attributes.Append(
                        xmlHelper.addAttribute(document, "name", name));
                    newNode.Attributes.Append(
                        xmlHelper.addAttribute(document, "type", type));

                    //Select for new node insert position
                    if (position == null || position == "end")
                    {
                        //Append new node at the end of root node
                        rootNode.AppendChild(newNode);

                        //Mark document modified
                        modified = true;
                    }
                    else if (position == "beginning")
                    {
                        //Prepend new node at the beginnig of root node
                        rootNode.PrependChild(newNode);

                        //Mark document modified
                        modified = true;
                    }
                }
            }

            #endregion

            #region IIS7

            //Set insert node default true
            insertNode = true;

            rootNode = document.SelectSingleNode("//configuration/system.webServer/modules");

            if (rootNode != null && name != null)
            {
                //Look for existing nodes with same path like the new node
                if (rootNode.HasChildNodes)
                {
                    //Look for existing nodeType nodes
                    XmlNode node = rootNode.SelectSingleNode(
                        String.Format("add[@name = '{0}']", name));

                    //If path already exists 
                    if (node != null)
                    {
                        //Cancel insert node operation
                        insertNode = false;
                    }
                }
                //Check for insert flag
                if (insertNode)
                {
                    //Create new add node with attributes
                    XmlNode newAddNode = document.CreateElement("add");
                    newAddNode.Attributes.Append(
                        xmlHelper.addAttribute(document, "name", name));
                    newAddNode.Attributes.Append(
                        xmlHelper.addAttribute(document, "type", type));
                

                    //Select for new node insert position
                    if (position == null || position == "end")
                    {
                        //Append new node at the end of root node
                        rootNode.AppendChild(newAddNode);

                        //Mark document modified
                        modified = true;
                    }
                    else if (position == "beginning")
                    {
                        //Prepend new node at the beginnig of root node
                        rootNode.PrependChild(newAddNode);

                        //Mark document modified
                        modified = true;
                    }
                }
            }

            #endregion

            //Check for modified document
            if (modified)
            {
                try
                {
                    //Save the Rewrite config file with the new rewerite rule
                    document.Save(HttpContext.Current.Server.MapPath(FULL_PATH));

                    //No errors so the result is true
                    result = true;
                }
                catch (Exception e)
                {
                    //Log error message
                    string message = "Error at execute AddHttpModule package action: " + e.Message;
                    LogHelper.Error(typeof(AddHttpModule), message, e);

                }
            }
            return result;
        }

        /// <summary>
        /// Removes the xmlData node from the web.config file
        /// </summary>
        /// <param name="packageName">Name of the package that we install</param>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <returns>True when succeeded</returns>
        public bool Undo(string packageName, System.Xml.XmlNode xmlData)
        {
            //Set result default to false
            bool result = false;

            //Get attribute values of xmlData
            string path, name;
            if (!getAttribute(xmlData, "path", out path)) return result;
            name = getAttributeDefault(xmlData, "name", null);

            //Create a new xml document
            XmlDocument document = new XmlDocument();

            //Keep current indentions format
            document.PreserveWhitespace = true;

            //Load the web.config file into the xml document
            document.Load(HttpContext.Current.Server.MapPath(FULL_PATH));

            //Set modified document default to false
            bool modified = false;

            #region IIS6

            //Select root node in the web.config file for insert new nodes
            XmlNode rootNode = document.SelectSingleNode("//configuration/system.web/httpModules");

            //Check for rootNode exists
            if (rootNode != null)
            {
                //Look for existing nodes with same path of undo attribute
                if (rootNode.HasChildNodes)
                {
                    //Look for existing add nodes with attribute path
                    foreach (XmlNode existingNode in rootNode.SelectNodes(
                        String.Format("add[@name = '{0}']", name)))
                    {
                        //Remove existing node from root node
                        rootNode.RemoveChild(existingNode);
                        modified = true;
                    }
                }
            }

            #endregion

            #region IIS7

            //Select root node in the web.config file for insert new nodes
            rootNode = document.SelectSingleNode("//configuration/system.webServer/modules");

            //Check for rootNode exists
            if (rootNode != null && name != null)
            {
                //Look for existing nodes with same path of undo attribute
                if (rootNode.HasChildNodes)
                {
                    //Look for existing add nodes with attribute path
                    foreach (XmlNode existingNode in rootNode.SelectNodes(String.Format("add[@name = '{0}']", name)))
                    {
                        //Remove existing node from root node
                        rootNode.RemoveChild(existingNode);
                        modified = true;
                    }
                }
            }

            #endregion

            if (modified)
            {
                try
                {
                    //Save the Rewrite config file with the new rewerite rule
                    document.Save(HttpContext.Current.Server.MapPath(FULL_PATH));

                    //No errors so the result is true
                    result = true;
                }
                catch (Exception e)
                {
                    //Log error message
                    string message = "Error at undo AddHttpHandler package action: " + e.Message;
                    LogHelper.Error(typeof(AddHttpModule), message, e);
                }
            }
            return result;
        }

        /// <summary>
        /// Get a named attribute from xmlData root node
        /// </summary>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <param name="value">returns the attribute value from xmlData</param>
        /// <returns>True, when attribute value available</returns>
        private bool getAttribute(XmlNode xmlData, string attribute, out string value)
        {
            //Set result default to false
            bool result = false;

            //Out params must be assigned
            value = String.Empty;

            //Search xml attribute
            XmlAttribute xmlAttribute = xmlData.Attributes[attribute];

            //When xml attribute exists
            if (xmlAttribute != null)
            {
                //Get xml attribute value
                value = xmlAttribute.Value;

                //Set result successful to true
                result = true;
            }
            else
            {
                //Log error message
                string message = "Error at AddHttpModule package action: "
                     + "Attribute \"" + attribute + "\" not found.";
                LogHelper.Warn(typeof(AddHttpModule), message);
            }
            return result;
        }

        /// <summary>
        /// Get an optional named attribute from xmlData root node
        /// when attribute is unavailable, return the default value
        /// </summary>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The attribute value or the default value</returns>
        private string getAttributeDefault(XmlNode xmlData, string attribute, string defaultValue)
        {
            //Set result default value
            string result = defaultValue;

            //Search xml attribute
            XmlAttribute xmlAttribute = xmlData.Attributes[attribute];

            //When xml attribute exists
            if (xmlAttribute != null)
            {
                //Get available xml attribute value
                result = xmlAttribute.Value;
            }
            return result;
        }

        /// <summary>
        /// Returns a Sample XML Node 
        /// In this case the Sample HTTP Module TimingModule 
        /// </summary>
        /// <returns>The sample xml as node</returns>
        public XmlNode SampleXml()
        {
            return umbraco.cms.businesslogic.packager.standardPackageActions.helper.parseStringToXmlNode(
                "<Action runat=\"install\" undo=\"true/false\" alias=\"AddHttpModule\" "
                    + "position=\"beginning/end\" "
                    + "type=\"umbraco.presentation.channels.api, umbraco\" "
                    + "name=\"UmbracoChannels\" />"
            );
        }

        #endregion
    }
}
