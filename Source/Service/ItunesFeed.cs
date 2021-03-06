﻿using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Service
{
    public class ItunesFeed : SyndicationFeed
    {
        #region Data Members

        private const string Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";
        private const string Prefix = "itunes";

        private readonly List<List<string>> _itunesCategories = new List<List<string>>();

        #endregion

        #region Constructors

        public ItunesFeed(string title, string description, Uri feedAlternateLink)
            : base(title, description, feedAlternateLink)
        {

        }

        #endregion

        #region Properties

        public string Subtitle { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string OwnerName { get; set; }

        public string OwnerEmail { get; set; }

        public bool Explicit { get; set; }

        #endregion

        #region SyndicationFeed

        protected override void WriteAttributeExtensions(XmlWriter writer, string version)
        {
            writer.WriteAttributeString("xmlns", Prefix, null, Namespace);
        }

        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            WriteItunesElement(writer, "subtitle", Subtitle);
            WriteItunesElement(writer, "author", Author);
            WriteItunesElement(writer, "summary", Summary);
            if (ImageUrl != null)
            {
                WriteItunesElement(writer, "image", ImageUrl.ToString());
            }
            WriteItunesElement(writer, "explicit", Explicit ? "yes" : "no");

            writer.WriteStartElement(Prefix, "owner", Namespace);
            WriteItunesElement(writer, "name", OwnerName);
            WriteItunesElement(writer, "email", OwnerEmail);
            writer.WriteEndElement();

            foreach (var category in _itunesCategories)
            {
                writer.WriteStartElement(Prefix, "category", Namespace);
                writer.WriteAttributeString("text", category[0]);
                if (category.Count == 2)
                {
                    writer.WriteStartElement(Prefix, "category", Namespace);
                    writer.WriteAttributeString("text", category[1]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        #endregion

        #region Methods

        private static void WriteItunesElement(XmlWriter writer, string name, string value)
        {
            if (value == null) return;

            writer.WriteStartElement(Prefix, name, Namespace);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        #endregion
    }
}
