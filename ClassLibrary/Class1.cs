﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassLibrary
{
    [Serializable]
    public class City
    {
        public City (string name, Population pop, int traf)
        {
            Name = name;
            People = pop;
            Traffic = traf;
        }

        public string Name { get; set; }
        public Population People { get; set; }
        public int Traffic { get; set; }
        public int Coefficient { get; set; }
    }

    [Serializable]
    public class Country : INotifyPropertyChanged
    {
        private int fund;
        private int week;
        private int newfund;
        public Dictionary<int, double> monthsIllnessRate = new Dictionary<int, double> { { 0, 1.2 }, { 1, 1.2 }, { 2, 1.3 }, { 3, 1.2 }, { 4, 1.1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1.3 }, { 9, 1.4 }, { 10, 1.5 }, { 11, 1.3 } };

        public string DiseaseName { get; set; }
        public string Name { get; set; }
        public int Months { get; set; }
        public int Month { get; set; }
        public int Cost { get; set; }
        public int Weeks { get; set; }
        public int CitiesCount { get; set; }
        public City[] Cities { get; set; }
        public int Fund
        {
            get { return fund; }
            set
            {
                fund = value;
                OnPropertyChanged("Fund");
            }
        }
        public int NewFund
        {
            get { return newfund; }
            set
            {
                newfund = value;
                OnPropertyChanged("NewFund");
            }
        }
        public int Week
        {
            get { return week; }
            set
            {
                week = value;
                OnPropertyChanged("Week");
            }
        }

        public Country(string name, string diseasename, int months, int month, int fund, int cost, int cities)
        {
            DiseaseName = diseasename;
            Name = name;
            Months = months;
            Month = month;
            Weeks = Months * 4;
            Fund = fund;
            Cost = cost;
            CitiesCount = cities;
            Cities = new City[cities];
            Week = 0;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void WeeklyUpdate()
        {
            Week++;
            if (Week % 4 == 0)
                Month++;
            Month %= 12;
            NewFund = 0;
            for (int i = 0; i < CitiesCount; i++)
            {
                if ((int)Math.Round(Cities[i].People.Ill * monthsIllnessRate[Month] * 0.23 * ((double)Cities[i].Traffic / 10 + 1) * (double)((Cities[i].People.Total - Cities[i].People.Ill - Cities[i].People.Immune) / Cities[i].People.Total) + Math.Ceiling(0.00002 * (Cities[i].People.Total - Cities[i].People.Ill - Cities[i].People.Immune)), MidpointRounding.AwayFromZero) > Cities[i].People.Total - Cities[i].People.Immune - Cities[i].People.Ill)
                    Cities[i].People.NewIll = Cities[i].People.Total - Cities[i].People.Immune - Cities[i].People.Ill;
                else
                    Cities[i].People.NewIll = (int)Math.Round(Cities[i].People.Ill* monthsIllnessRate[Month] * 0.23 * ((double)Cities[i].Traffic / 10 + 1) * (((double)Cities[i].People.Total - (double)Cities[i].People.Ill - (double)Cities[i].People.Immune) / (double)Cities[i].People.Total) + Math.Ceiling(0.00002 * (Cities[i].People.Total - Cities[i].People.Ill - Cities[i].People.Immune)), MidpointRounding.AwayFromZero);
                Cities[i].People.Ill = Cities[i].People.Ill + Cities[i].People.NewIll - Cities[i].People.ImmuneSchedule[Week];
                Cities[i].People.ImmuneSchedule[Week + 2] += (int)Math.Round(Cities[i].People.NewIll * 0.25, MidpointRounding.AwayFromZero);
                Cities[i].People.ImmuneSchedule[Week + 3] += (int)Math.Round(Cities[i].People.NewIll * 0.65);
                Cities[i].People.ImmuneSchedule[Week + 4] += Cities[i].People.NewIll - (int)Math.Round(Cities[i].People.NewIll * 0.25, MidpointRounding.AwayFromZero) - (int)Math.Round(Cities[i].People.NewIll * 0.65);
                Cities[i].People.NewRecovered = Cities[i].People.ImmuneSchedule[Week];
                Cities[i].People.Recovered += Cities[i].People.NewRecovered;
                Cities[i].People.Vaccinated += Cities[i].People.NewVaccinated;
                Cities[i].People.Immune += Cities[i].People.NewRecovered + Cities[i].People.NewVaccinated;
                Cities[i].People.NewVaccinated = 0;
                NewFund += (int)Math.Round((Cities[i].People.Total - Cities[i].People.Ill) * 0.65 * 0.13) - (int)Math.Round((Cities[i].People.Ill * 0.65 * 0.5));
            }
            Fund += NewFund;
        }
    }

    [Serializable]
    public class Population : INotifyPropertyChanged
    {
        private int ill;
        private int newIll;
        private int vaccinated;
        private int newVaccinated;
        private int recovered;
        private int newRecovered;
        private int immune;

        public int Total { get; set; }
        public int[] ImmuneSchedule { get; set; }
        public int Ill
        {
            get { return ill; }
            set
            {
                ill = value;
                OnPropertyChanged("Ill");
            }
        }
        public int NewIll
        {
            get { return newIll; }
            set
            {
                newIll = value;
                OnPropertyChanged("NewIll");
            }
        }
        public int Vaccinated
        {
            get { return vaccinated; }
            set
            {
                vaccinated = value;
                OnPropertyChanged("Vaccinated");
            }
        }
        public int NewVaccinated
        {
            get { return newVaccinated; }
            set
            {
                newVaccinated = value;
                OnPropertyChanged("NewVaccinated");
            }
        }
        public int Recovered
        {
            get { return recovered; }
            set
            {
                recovered = value;
                OnPropertyChanged("Recovered");
            }
        }
        public int NewRecovered
        {
            get { return newRecovered; }
            set
            {
                newRecovered = value;
                OnPropertyChanged("NewRecovered");
            }
        }
        public int Immune
        {
            get { return immune; }
            set
            {
                immune = value;
                OnPropertyChanged("Immune");
            }
        }

        public Population(int tot, int ill, int vac, int weeks)
        {
            Total = tot;
            Ill = ill;
            Vaccinated = vac;
            Immune = Vaccinated;
            ImmuneSchedule = new int[weeks+5];
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}