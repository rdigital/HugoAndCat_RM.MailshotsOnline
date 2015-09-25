using System;
using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IDataService
    {
        Models.IDistributionList GetDistributionListForUser(int userId, Guid distributionListId);
        IEnumerable<IDistributionList> GetDistributionListsForUser(int userId);
        IEnumerable<IDistributionList> GetDistributionLists(Func<IDistributionList, bool> filter);
        IDistributionList SaveDistributionList(IDistributionList distributionList);

        IDistributionList CreateDistributionList(IMember member, string listName, byte[] bytes, string contentType,
                                                 Enums.DistributionListFileType fileType);

        IDistributionList UpdateDistributionList(IDistributionList distributionList, byte[] bytes, string contentType,
                                                 Enums.DistributionListFileType fileType);

        List<string> GetFirstTwoLinesOfWorkingFile(IDistributionList distributionList);
    }
}