
using Realms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Wtd.Core.Models;
using Wtd.Core.Services;
using Xamarin.Forms;
using System;

namespace Wtd.Core.ViewModels
{
    public class HelpViewModel : BaseModel
    {
        private readonly Realm _realm;
        private string _topic;

        public ICommand CloseCommand { get; }

        public ImageSource CloseIcon { get { return ImageSource.FromFile("close.png"); } }

        public string Title { get; set; }

        public IEnumerable<HelpSection> Sections { get; }

        public HelpViewModel(Realm realm, string topic)
        {
            _realm = realm;
            _topic = topic;
            CloseCommand = new Command(Close);

            Sections= GetHelp();
        }

        private List<HelpSection> GetHelp()
        {

           var sections = new List<HelpSection>();

           var helpTitle = _realm.All<HelpTitle>().Where(h => h.Topic==_topic).FirstOrDefault();

            if (helpTitle == null)
            {
                LoadHelp();
                GetHelp();
            }
            else
            {
                Title = helpTitle.Text;
                return _realm.All<HelpSection>().Where(h => h.Topic == _topic).OrderBy(h => h.DisplayOrder).ToList();                     
            }

            return null;
        }

        private void LoadHelp()
        {
            SetJobCalendarHelp();
            SetPlantHelp();
            SetFrostHelp();
            SetBasketHelp();
            SetReportHelp();
            SetEditJobHelp();
            SetEditPlantHelp();
            SetEditFrostHelp();
        }

        private void SetJobCalendarHelp()
        {
            var helpTitle = new HelpTitle { Topic = "main", Text = "Job Calendar" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "main", Text = "This view gives a list of jobs to do for the month selected.", Icon = "job.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Jobs can be associated with palnts and can of of three types; Cultivate, Plant and Other", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Click this icon to create a new job entry.", Icon = "add.png", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            helpSection = new HelpSection { Topic = "main", Text = "Alternatively click on the calendar date to enter a job for that date.", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Click on a job list entry to edit.", DisplayOrder = 4 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Click this icon to plant/variety/season reviews.", Icon = "report.png", DisplayOrder = 5 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Click this icon view/add a yield entry.", Icon = "basket.png", DisplayOrder = 6 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "main", Text = "Click this icon view/add a plant entry.", Icon = "plant.png", DisplayOrder = 7 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
        }

        private void SetPlantHelp()
        {
            var helpTitle = new HelpTitle { Topic = "plant", Text = "Plants" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "plant", Text = "This view gives a list of plants", Icon = "plant.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "plant", Text = "Click this icon to create a new plant entry.", Icon = "add.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "plant", Text = "Note the default variety is 'Common'", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "plant", Text = "Click on a plant list entry to edit.", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "plant", Text = "Click this icon to return to Job Calendar view.", Icon = "job.png", DisplayOrder = 4 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
        }

        private void SetFrostHelp()
        {
            var helpTitle = new HelpTitle { Topic = "frost", Text = "Frost" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "frost", Text = "This view gives a list of frost dates which can be filtered by season", Icon = "frost.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "frost", Text = "Click this icon to create a new frost entry.", Icon = "add.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "frost", Text = "Click on a frost list entry to edit.", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "frost", Text = "Click this icon to return to Job Calendar view.", Icon = "job.png", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));


        }

        private void SetBasketHelp()
        {
            var helpTitle = new HelpTitle { Topic = "yield", Text = "Yield" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "yield", Text = "This view gives a list of plant varieties and their yield for a season", Icon = "basket.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "yield", Text = "Click this icon to create a new yield entry.", Icon = "add.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "yield", Text = "Click on a yield list entry to edit.", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "yield", Text = "Click this icon to return to Job Calendar view.", Icon = "job.png", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));            
        }

        private void SetReportHelp()
        {
            var helpTitle = new HelpTitle { Topic = "report", Text = "Review Report" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "report", Text = "This view gives a report reviewing notes based on plant and season", Icon = "report.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "report", Text = "The report can be filtered by plant, season and job type.", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

             helpSection = new HelpSection { Topic = "report", Text = "Click this icon to return to Job Calendar view.", Icon = "job.png", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
        }

        private void SetEditJobHelp()
        {
            var helpTitle = new HelpTitle { Topic = "edit_job", Text = "Edit Job" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "edit_job", Text = "This view is used to add, delete and edit a job entry", Icon = "job.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_job", Text = "Click this icon to save a new job entry.", Icon = "save.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_job", Text = "Click this icon to delete the selected job entry.", Icon = "delete.png", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_job", Text = "Briefly describe the job.", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_job", Text = "Enter a date  for the completion of the job.", DisplayOrder = 4 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_job", Text = "Select the job type.", DisplayOrder = 5 };
            _realm.Write(() => _realm.Add(helpSection, update: true));  

            helpSection = new HelpSection { Topic = "edit_job", Text = "Associate the job with a plant if desired.", DisplayOrder = 6 };
            _realm.Write(() => _realm.Add(helpSection, update: true)); 
            
            helpSection = new HelpSection { Topic = "edit_job", Text = "Add any notes about this job if necessary.", DisplayOrder = 7 };
            _realm.Write(() => _realm.Add(helpSection, update: true));             
        }
        
        private void SetEditPlantHelp()
        {
            var helpTitle = new HelpTitle { Topic = "edit_plant", Text = "Edit Plant" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "edit_plant", Text = "This view is used to add, delete and edit a plant entry", Icon = "plant.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_plant", Text = "Click this icon to save a new plant entry.", Icon = "save.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_plant", Text = "Click this icon to delete the selected plant entry.", Icon = "delete.png", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_plant", Text = "Plant name.", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_plant", Text = "Plant variety (default is 'Common').", DisplayOrder = 4 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_plant", Text = "Add any notes about this plant/variety if necessary.", DisplayOrder = 5 };
            _realm.Write(() => _realm.Add(helpSection, update: true));             
        }
        
        private void SetEditFrostHelp()
        {
            var helpTitle = new HelpTitle { Topic = "edit_frost", Text = "Edit Frost" };
            _realm.Write(() => _realm.Add(helpTitle, update: true));

            var helpSection = new HelpSection { Topic = "edit_frost", Text = "This view is used to add, delete and edit a frost entry", Icon = "frost.png", DisplayOrder = 0 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_frost", Text = "Click this icon to save a new frost entry.", Icon = "save.png", DisplayOrder = 1 };
            _realm.Write(() => _realm.Add(helpSection, update: true));

            helpSection = new HelpSection { Topic = "edit_frost", Text = "Click this icon to delete the selected frost entry.", Icon = "delete.png", DisplayOrder = 2 };
            _realm.Write(() => _realm.Add(helpSection, update: true));
            
            helpSection = new HelpSection { Topic = "edit_frost", Text = "Date.", DisplayOrder = 3 };
            _realm.Write(() => _realm.Add(helpSection, update: true));                  
        }
        
        private void Close()
        {           
            NavigationService.Navigate(true);
        }
    }
}
