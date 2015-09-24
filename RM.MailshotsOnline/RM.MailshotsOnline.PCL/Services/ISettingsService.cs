using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ISettingsService
    {
        ISettingsFromCms GetCurrentSettings();

        ISettingsFromCms UpdateCurrentSettings(
            decimal vatRate, 
            decimal msolFee, 
            decimal pricePerRentedDataUnit, 
            decimal dataServiceFee, 
            int umbracoContentId, 
            int moderationTimeEstimate, 
            int printingTimeEstimate,
            string publicHolidayDates);
    }
}
