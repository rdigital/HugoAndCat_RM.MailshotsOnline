﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using HC.RM.Common;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Business.Processors
{
    public class DistributionListProcessor
    {
        private readonly ILogger _logger;
        private readonly string _className;

        public DistributionListProcessor(ILogger logger)
        {
            _logger = logger;
            _className = GetType().Name;
        }

        public ModifyListConfirmFieldsModel AttemptToMapDataToColumns(IDistributionList list,
                                                                      DataMappingFolder dataMappings,
                                                                      byte[] csvBytes)
        {
            var confirmFieldsModel = new ModifyListConfirmFieldsModel
                                     {
                                         DistributionListId = list.DistributionListId,
                                         ListName = list.Name,
                                         FirstRowIsHeader = null,
                                         MappingOptions =
                                             dataMappings.Mappings.ToDictionary(m => m.FieldName, m => m.Name),
                                     };

            using (var stream = new MemoryStream(csvBytes))
            {
                using (var sr = new StreamReader(stream))
                {
                    // Assume No Header Row to start with.
                    using (var csv = new CsvReader(sr, new CsvConfiguration {HasHeaderRecord = false}))
                    {
                        int rows = 0;
                        int columns = 0;

                        List<KeyValuePair<string, string>> items = null;

                        while (rows < 2)
                        {
                            try
                            {
                                csv.Read();

                                if (rows == 0)
                                {
                                    columns = csv.CurrentRecord.Length;
                                    confirmFieldsModel.ColumnCount = columns;
                                    confirmFieldsModel.FirstTwoRowsWithGuessedMappings =
                                        new List<Tuple<string, string, string>>(columns);
                                    items = new List<KeyValuePair<string, string>>(columns);
                                }

                                for (int column = 0; column < columns; column++)
                                {
                                    if (rows == 0)
                                    {
                                        // First Row
                                        // Grab Value and see if we can find a mapping for it:
                                        var possibleHeading = csv.GetField(column);

                                        var possibleMapping =
                                            dataMappings.Mappings.FirstOrDefault(
                                                                                 m =>
                                                                                     m.FieldMappings.Contains(
                                                                                                              possibleHeading
                                                                                                                  .ToLower
                                                                                                                  ()
                                                                                                                  .Trim()));

                                        if (possibleMapping != null)
                                        {
                                            // We think we might have a heading row
                                            confirmFieldsModel.FirstRowIsHeader = true;
                                        }

                                        // ReSharper disable once PossibleNullReferenceException
                                        items.Add(new KeyValuePair<string, string>(possibleHeading,
                                                                                   possibleMapping?.FieldName));
                                    }
                                    else
                                    {
                                        var value = csv.GetField(column);

                                        // ReSharper disable once PossibleNullReferenceException
                                        confirmFieldsModel.FirstTwoRowsWithGuessedMappings.Add(
                                                                                               new Tuple
                                                                                                   <string, string,
                                                                                                   string>(
                                                                                                   items[column].Key,
                                                                                                   value,
                                                                                                   items[column].Value));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Exception(_className, "ShowConfirmFiledsForm", ex);
                                throw;
                            }

                            rows++;
                        }
                    }
                }
            }
            return confirmFieldsModel;
        }

        public ModifyListMappedFieldsModel<T> BuildListsFromFieldMappings<T>(IDistributionList list,
                                                                             List<string> mappings,
                                                                             int columnCount,
                                                                             bool firstRowIsHeader,
                                                                             byte[] csvBytes)
            where T : IDistributionContact
        {
            var validContacts = new Dictionary<string, T>();
            var duplicateContacts = new List<T>();
            var errorContacts = new List<T>();

            // Build CSV Map
            var contactMap = new DefaultCsvClassMap<T>();

            // mapping holds the Property - csv column mapping 
            for (int mappingIndex = 0; mappingIndex < columnCount; mappingIndex++)
            {
                var columnName = mappings[mappingIndex];
                if (!string.IsNullOrEmpty(columnName))
                {
                    var propertyInfo = typeof (T).GetProperty(columnName);
                    var newMap = new CsvPropertyMap(propertyInfo);
                    newMap.Index(mappingIndex);
                    contactMap.PropertyMaps.Add(newMap);
                }
            }

            // Should already have returned if FirstRowIsHeader is null.
            var csvConfig = new CsvConfiguration {HasHeaderRecord = firstRowIsHeader};

            csvConfig.RegisterClassMap(contactMap);

            using (var stream = new MemoryStream(csvBytes))
            {
                using (var sr = new StreamReader(stream))
                {
                    // Assume No Header Row to start with.
                    using (var csv = new CsvReader(sr, csvConfig))
                    {
                        while (csv.Read())
                        {
                            var contact = csv.GetRecord<T>();

                            if (contact != null)
                            {
                                contact.ContactId = Guid.NewGuid();
                                ICollection<ValidationResult> results;
                                bool isValid = contact.TryValidate(out results);

                                // TODO: Dedupe against existing list as well
                                if (isValid && !validContacts.ContainsKey(contact.AddressRef))
                                {
                                    validContacts.Add(contact.AddressRef, contact);
                                }
                                else if (!isValid)
                                {
                                    errorContacts.Add(contact);
                                }
                                else
                                {
                                    duplicateContacts.Add(contact);
                                }
                            }
                        }
                    }
                }
            }

            var mappedFields = new ModifyListMappedFieldsModel<T>
                               {
                                   ValidContactsCount = validContacts.Count,
                                   ValidContacts = validContacts.Select(vc => vc.Value),
                                   InvalidContactsCount = errorContacts.Count,
                                   InvalidContacts = errorContacts,
                                   DuplicateContactsCount = duplicateContacts.Count,
                                   DuplicateContacts = duplicateContacts
                               };

            return mappedFields;
        }
    }
}