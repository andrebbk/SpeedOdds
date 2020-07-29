using SpeedOdds.Commons.Enums;
using SpeedOdds.Models;
using SpeedOdds.Models.Shared;
using SpeedOdds.UserControls.DrawableMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpeedOdds.Windows
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ChooseTeamsWindow : Window
    {
        private DrawableMenuTypeValues _typeDrawable;
        private UserControl_DrawableMenuMatches _parentMatches;
        private UserControl_DrawableMenuTeams _parentTeams;

        public ChooseTeamsWindow(UserControl _parent, DrawableMenuTypeValues typeDrawable, IEnumerable<Team> _equipas)
        {
            InitializeComponent();

            _typeDrawable = typeDrawable;
            if (typeDrawable == DrawableMenuTypeValues.Matches)
            {
                _parentMatches = (UserControl_DrawableMenuMatches)_parent;
            }
            else if (typeDrawable == DrawableMenuTypeValues.Teams)
            {
                _parentTeams = (UserControl_DrawableMenuTeams)_parent;
            }

            if (_equipas != null && _equipas != null? _equipas.Count() > 0 : false)
            {
                ObservableCollection<TeamComboModel> teamsBox = new ObservableCollection<TeamComboModel>();
                foreach (var item in _equipas) teamsBox.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                ListBoxTeams.ItemsSource = teamsBox;

                //check for already selected
                _typeDrawable = typeDrawable;
                if (typeDrawable == DrawableMenuTypeValues.Matches)
                {
                    if(_parentMatches.teamsListToFilter != null && 
                        _parentMatches.teamsListToFilter != null? _parentMatches.teamsListToFilter.Count() > 0 : false)
                    {
                        foreach (var boundObject in teamsBox.Where(t => _parentMatches.teamsListToFilter.Contains(t.TeamId)))
                        {
                            ListBoxTeams.SelectedItems.Add(boundObject);
                        }
                    }
                }
                else if (typeDrawable == DrawableMenuTypeValues.Teams)
                {
                    if (_parentTeams.teamsListToFilter != null &&
                        _parentTeams.teamsListToFilter != null ? _parentTeams.teamsListToFilter.Count() > 0 : false)
                    {
                        foreach (var boundObject in teamsBox.Where(t => _parentTeams.teamsListToFilter.Contains(t.TeamId)))
                        {
                            ListBoxTeams.SelectedItems.Add(boundObject);
                        }
                    }
                }                
            }           
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in ListBoxTeams.SelectedItems)
            {
                if((TeamComboModel)item != null)
                {
                    if (_typeDrawable == DrawableMenuTypeValues.Matches)
                    {
                        if(_parentMatches.teamsListToFilter == null) _parentMatches.teamsListToFilter = new List<int>();
                        _parentMatches.teamsListToFilter.Add(((TeamComboModel)item).TeamId);
                    }
                    else if (_typeDrawable == DrawableMenuTypeValues.Teams)
                    {
                        if (_parentTeams.teamsListToFilter == null) _parentTeams.teamsListToFilter = new List<int>();
                        _parentTeams.teamsListToFilter.Add(((TeamComboModel)item).TeamId);
                    }                    
                }
            }

            this.DialogResult = true;
            this.Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            if (_typeDrawable == DrawableMenuTypeValues.Matches)
            {
                _parentMatches.teamsListToFilter = null;
            }
            else if (_typeDrawable == DrawableMenuTypeValues.Teams)
            {
                _parentTeams.teamsListToFilter = null;
            }
            this.DialogResult = false;
            this.Close();
        }
    }
}
