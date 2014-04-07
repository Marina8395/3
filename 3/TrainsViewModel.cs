using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3
{
    public class TrainsViewModel
    {
        private Trains timetable;
        public ObservableCollection<Train> Trains { get; private set; }

        public TrainsViewModel()
        {
            timetable = new Trains();
            Trains = new ObservableCollection<Train>();
        }

        public void Load()
        {
            timetable.LoadData();

            foreach (var train in timetable.trains)
            {
                Trains.Add(train);
            }
        }

        public void Save()
        {
            timetable.SaveData(Trains.ToList<Train>());
        }
    }
}
