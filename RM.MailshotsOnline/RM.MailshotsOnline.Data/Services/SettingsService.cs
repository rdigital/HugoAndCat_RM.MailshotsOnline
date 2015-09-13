using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class SettingsService : ISettingsService
    {
        private StorageContext _context;

        public SettingsService()
        {
            _context = new StorageContext();
        }

        public SettingsService(string connectionStringName)
        {
            _context = new StorageContext(connectionStringName);
        }

        public ISettingsFromCms GetCurrentSettings()
        {
            return _context.Settings.OrderByDescending(s => s.CreatedDate).FirstOrDefault(s => s.Active == true);
        }

        public ISettingsFromCms UpdateCurrentSettings(decimal vatRate, decimal msolFee, decimal pricePerRentedDataUnit, decimal dataServiceFee, int umbracoContentId)
        {
            var activeSettings = _context.Settings.Where(s => s.Active == true);
            foreach (var setting in activeSettings)
            {
                setting.Active = false;
                setting.UpdatedDate = DateTime.UtcNow;
            }

            var newSetting = new SettingsFromCms();
            newSetting.MsolPerUseFee = msolFee;
            newSetting.VatRate = vatRate;
            newSetting.PricePerRentedDataRecord = pricePerRentedDataUnit;
            newSetting.DataRentalServiceFee = dataServiceFee;
            newSetting.Active = true;
            newSetting.UmbracoContentId = umbracoContentId;

            _context.Settings.Add(newSetting);

            _context.SaveChanges();

            return newSetting;
        }
    }
}
