using System;
using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Gets a specific distribution list for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="distributionListId">The distribution list identifier.</param>
        /// <returns></returns>
        IDistributionList GetDistributionListForUser(int userId, Guid distributionListId);

        /// <summary>
        /// Gets all distribution lists for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<IDistributionList> GetDistributionListsForUser(int userId);

        /// <summary>
        /// Checks to see if the supplied list name is already in use for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        bool ListNameIsAlreadyInUse(int userId, string listName);

        /// <summary>
        /// Gets distribution lists.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<IDistributionList> GetDistributionLists(Func<IDistributionList, bool> filter);

        /// <summary>
        /// Saves the distribution list back to the data store.
        /// </summary>
        /// <param name="distributionList">The distribution list.</param>
        /// <returns></returns>
        IDistributionList SaveDistributionList(IDistributionList distributionList);

        /// <summary>
        /// Creates a new distribution list with the supplied data file.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listState">State of the list.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        IDistributionList CreateDistributionList(IMember member, string listName, Enums.DistributionListState listState,
                                                 byte[] bytes, string contentType,
                                                 Enums.DistributionListFileType fileType);

        /// <summary>
        /// Updates a distribution list with the supplied data file.
        /// </summary>
        /// <param name="distributionList">The distribution list.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        IDistributionList UpdateDistributionList(IDistributionList distributionList, byte[] bytes, string contentType,
                                                 Enums.DistributionListFileType fileType);

        /// <summary>
        /// Gets a specific data file from a list.
        /// </summary>
        /// <param name="distributionList">The distribution list.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        byte[] GetDataFile(IDistributionList distributionList, Enums.DistributionListFileType fileType);

        /// <summary>
        /// If there are working or error files against the list, these are removed. If there is no final file on the list it is deleted.
        /// </summary>
        /// <param name="distributionList">The distribution list.</param>
        void AbandonContactEdits(IDistributionList distributionList);

        /// <summary>
        /// Creates a "working" XML file from a list of contacts.
        /// </summary>
        /// <typeparam name="T">The concrete type of the contacts</typeparam>
        /// <param name="distributionList">The distribution list.</param>
        /// <param name="contactsCount">The contacts count.</param>
        /// <param name="contacts">The contacts.</param>
        /// <returns></returns>
        IDistributionList CreateWorkingXml<T>(IDistributionList distributionList, int contactsCount,
                                           IEnumerable<IDistributionContact> contacts) where T: IDistributionContact;

        /// <summary>
        /// Creates an "errors" XML file with from lists of duplicate and invalid contacts.
        /// </summary>
        /// <typeparam name="T">The concrete type of the contacts</typeparam>
        /// <param name="distributionList">The distribution list.</param>
        /// <param name="errorsCount">The errors count.</param>
        /// <param name="errorContacts">The error contacts.</param>
        /// <param name="duplicatesCount">The duplicates count.</param>
        /// <param name="duplicateContacts">The duplicate contacts.</param>
        /// <returns></returns>
        IDistributionList CreateErrorXml<T>(IDistributionList distributionList, int errorsCount,
                                         IEnumerable<IDistributionContact> errorContacts, int duplicatesCount,
                                         IEnumerable<IDistributionContact> duplicateContacts) where T : IDistributionContact;

        /// <summary>
        /// Creates the "summary" model from the working and errors files.
        /// </summary>
        /// <typeparam name="T">The concrete type of the contacts</typeparam>
        /// <param name="distributionList">The distribution list.</param>
        /// <returns></returns>
        IModifyListSummaryModel<T> CreateSummaryModel<T>(IDistributionList distributionList)
            where T : IDistributionContact;

       IDistributionList CompleteContactEdits(IDistributionList distributionList);
    }
}