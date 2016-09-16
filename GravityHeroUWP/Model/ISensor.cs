using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GravityHero;
using Microsoft.Band.Sensors;

namespace GravityHeroUWP.Model
{
    public interface ISensor<TSensor, TReading> where TSensor : IBandSensor<TReading> where TReading : IBandSensorReading 
    {
        void Init(TimeSpan reportingInterval);
        void ReadingChanged(object sender, BandSensorReadingEventArgs<TReading> readingEvent);
    }
}
