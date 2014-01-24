using System;

namespace IRUmbraco
{
    using System.Web;
    using System.Xml;

    using global::Umbraco.Core.Logging;

    using umbraco;

    using Umbraco.Core;

    using umbraco.interfaces;

    using helper = umbraco.cms.businesslogic.packager.standardPackageActions.helper;

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
            return "IRBundle.AddHttpModule";
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
            string name;
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

    public class AddConfigSection : IPackageAction
    {
        #region IPackageAction AddConfigSection

        const string FULL_PATH = "/web.config";

        /// <summary>
        /// This Alias must be unique and is used as an identifier that must match 
        /// the alias in the package action XML
        /// </summary>
        /// <returns>The Alias in string format</returns>
        public string Alias()
        {
            return "IRBundle.AddConfigSection";
        }

        /// <summary>
        /// Append the xmlData node to the web.config file
        /// </summary>
        /// <param name="packageName">Name of the package that we install</param>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <returns>True when succeeded</returns>
        public bool Execute(string packageName, XmlNode xmlData)
        {
            bool result = false;

            string name, type, sectionGroup, requirePermission;
            if (!this.GetAttribute(xmlData, "name", out name) || !this.GetAttribute(xmlData, "type", out type))
            {
                return result;
            }
            this.GetAttribute(xmlData, "requirePermission", out requirePermission);
            this.GetAttribute(xmlData, "sectionGroup", out sectionGroup);

            var document = new XmlDocument { PreserveWhitespace = true };

            document.Load(HttpContext.Current.Server.MapPath(FULL_PATH));

            var xPath = "//configSections";

            if (!string.IsNullOrEmpty(sectionGroup))
            {
                xPath = string.Format("//configSections/sectionGroup[@name = '{0}']", sectionGroup);
            }

            XmlNode rootNode = document.SelectSingleNode(xPath);

            if (rootNode == null) return result;

            bool modified = false;

            if (rootNode.SelectSingleNode(string.Format("section[@name = '{0}']", name)) == null)
            {
                XmlNode newNode = document.CreateElement("section");
                newNode.Attributes.Append(
                    XmlHelper.AddAttribute(document, "name", name));
                newNode.Attributes.Append(
                    XmlHelper.AddAttribute(document, "type", type));
                if (!string.IsNullOrEmpty(requirePermission))
                {
                    newNode.Attributes.Append(XmlHelper.AddAttribute(document, "requirePermission", requirePermission));
                }
                rootNode.AppendChild(newNode);
                modified = true;
            }

            if (modified)
            {
                try
                {
                    document.Save(HttpContext.Current.Server.MapPath(FULL_PATH));
                    result = true;
                }
                catch (Exception e)
                {
                    string message = "Error at execute AddConfigSection package action: " + e.Message;
                    LogHelper.Error(typeof(AddConfigSection), message, e);
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
            bool result = false;

            string name, type, sectionGroup;
            if (!this.GetAttribute(xmlData, "name", out name) || !this.GetAttribute(xmlData, "type", out type))
            {
                return result;
            }

            this.GetAttribute(xmlData, "sectionGroup", out sectionGroup);

            var document = new XmlDocument { PreserveWhitespace = true };

            document.Load(HttpContext.Current.Server.MapPath(FULL_PATH));

            var xPath = "//configSections";

            if (!string.IsNullOrEmpty(sectionGroup))
            {
                xPath = string.Format("//configSections/sectionGroup[@name = '{0}']", sectionGroup);
            }

            XmlNode rootNode = document.SelectSingleNode(xPath);

            if (rootNode == null) return result;

            bool modified = false;

            if (rootNode.SelectSingleNode(string.Format("section[@name = '{0}']", name)) != null)
            {
                rootNode.RemoveChild(
                    rootNode.SelectSingleNode(string.Format("section[@name = '{0}']", name)));

                modified = true;
            }

            if (modified)
            {
                try
                {
                    document.Save(HttpContext.Current.Server.MapPath(FULL_PATH));
                    result = true;
                }
                catch (Exception e)
                {
                    string message = "Error at execute AddConfigSection package action: " + e.Message;
                    LogHelper.Error(typeof(AddConfigSection), message, e);
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
                string message = "Error at AddConfigSection package action: "
                     + "Attribute \"" + attribute + "\" not found.";
                LogHelper.Warn(typeof(AddConfigSection), message);
            }
            return result;
        }

        /// <summary>
        /// Returns a Sample XML Node 
        /// In this case we are adding the System.Web.Optimization namespace
        /// </summary>
        /// <returns>The sample xml as node</returns>
        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(
                "<Action runat=\"install\" undo=\"true/false\" alias=\"Umbundle.AddConfigSection\" "
                    + "name=\"core\" type=\"BundleTransformer.Core.Configuration.CoreSettings, BundleTransformer.Core\" sectionGroup=\"myGroup\""
                    + " />");
        }

        #endregion
    }


}
