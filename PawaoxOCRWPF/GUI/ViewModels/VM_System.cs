using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.GUI.MVVM;
using PawaoxOCRWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.ViewModels
{
    public class VM_System : ViewModel
    {
        private List<TabModel> _originalTabModels = new List<TabModel>();
        private List<TabModel> _hiddenTabModels = new List<TabModel>();

        private bool _isMenuOpen;
        public bool IsMenuOpen
        {
            get { return _isMenuOpen; }
            set { SetValue(ref _isMenuOpen, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetValue(ref _searchText, value); }
        }

        private List<TabModel> _tabModels;
        public List<TabModel> TabModels
        {
            get { return _tabModels; }
            set { SetValue(ref _tabModels, value); }
        }

        private TabModel _seletedTabModel;
        public TabModel SelectedTabModel
        {
            get { return _seletedTabModel; }
            set { SetValue(ref _seletedTabModel, value); }
        }

        private bool _includeHiddenTabs;
        public bool IncludeHiddenTabs
        {
            get { return _includeHiddenTabs; }
            set { SetValue(ref _includeHiddenTabs, value); }
        }
        public RelayCommand CommandToggleMenu { get; set; }
        public RelayCommand CommandMenuSearch { get; set; }
        public RelayCommand<TabModel> CommandSelectTab { get; set; }


        public RelayCommand<TabModel> CommandToggleTabVisibility { get; set; }
        public RelayCommand CommandToggleIncludeHiddenTabs { get; set; }

        public VM_System() : base(UserControlType.NONE)
        {
            CommandMenuSearch = new RelayCommand(MenuSearch);
            CommandToggleMenu = new RelayCommand(ToggleMenu);
            CommandSelectTab = new RelayCommand<TabModel>(SelectTab);

            CommandToggleTabVisibility = new RelayCommand<TabModel>(ToggleTabVisibility);
            CommandToggleIncludeHiddenTabs = new RelayCommand(ToggleIncludeHiddenTabs);

            IsMenuOpen = true;
            _tabModels = new List<TabModel>();
        }

        private void SelectTab(TabModel model)
        {
            if (model?.Type != null)
            {
                if (SelectedTabModel != null)
                    SelectedTabModel.IsSelected = false;
                model.IsSelected = true;
                SelectedTabModel = model;

                MessageBroker.Send(new MSG_ChangeUserControl(model.Type));
            }
        }

        private void ToggleMenu()
        {
            IsMenuOpen = !IsMenuOpen;
        }

        private void MenuSearch()
        {
            List<TabModel> models = new List<TabModel>();
            if (string.IsNullOrEmpty(SearchText))
            {
                models.AddRange(_originalTabModels);
                if (IncludeHiddenTabs)
                    models.AddRange(_hiddenTabModels);
            }
            else
            {
                foreach (TabModel tab in _originalTabModels)
                {
                    if (!string.IsNullOrEmpty(tab.Header))
                        if (tab.Header.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            models.Add(tab);
                }

                if (IncludeHiddenTabs)
                {
                    foreach (TabModel tab in _hiddenTabModels)
                    {
                        if (!string.IsNullOrEmpty(tab.Header))
                            if (tab.Header.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                models.Add(tab);
                    }
                }
            }

            models.Sort((a, b) => (a.Header ?? "").CompareTo(b.Header));
            this.TabModels = models;
        }

        public override void OnLoaded()
        {
            if (_originalTabModels.Count == 0)
            {

                MessageBroker.Send(new MSG_ChangeUserControl(UserControlType.MAIN));

                HashSet<string> hiddenTabs = new HashSet<string>();
#warning: TODO: Load hidden tabs from config?

                CreateTab(ref hiddenTabs, UserControlType.OCR, "Screen OCR", "Read text from the screen");

                List<TabModel> initialModels = new List<TabModel>();
                initialModels.AddRange(_originalTabModels);
                if (IncludeHiddenTabs)
                    initialModels.AddRange(_hiddenTabModels);

                initialModels.Sort((a, b) => (a.Header ?? "").CompareTo(b.Header));
                this.TabModels = initialModels;
            }
        }

        private void CreateTab(ref HashSet<string> hiddenTabs, UserControlType type, string header, string explanation = "")
        {
            TabModel tm = new TabModel() { Type = type, Header = header, Explanation = explanation };

            if (hiddenTabs.Contains(type.ToString()))
            {
                tm.IsHidden = true;
                _hiddenTabModels.Add(tm);
            }
            else
            {
                tm.IsHidden = false;
                _originalTabModels.Add(tm);
            }

        }

        private async void ToggleTabVisibility(TabModel tabModel)
        {
            if (tabModel == null)
                return;

            try
            {
                if (tabModel.IsHidden)
                {
                    tabModel.IsHidden = false;
                    _hiddenTabModels.Remove(tabModel);
                    _originalTabModels.Add(tabModel);
                }
                else
                {
                    tabModel.IsHidden = true;
                    _originalTabModels.Remove(tabModel);
                    _hiddenTabModels.Add(tabModel);
                }

#warning: TODO: Store in Config
                MenuSearch();
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void ToggleIncludeHiddenTabs()
        {
            this.IncludeHiddenTabs = !IncludeHiddenTabs;
            MenuSearch();
        }

        public override void OnUnloaded()
        {
        }
    }
}