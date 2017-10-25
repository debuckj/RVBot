﻿using System;

namespace WarframeNET
{
    public class EarthCycle
    {
        public long id { get; set; }
        public DateTime expiry { get; set; }
        public bool isDay { get; set; }
        public string timeLeft { get; set; }

        public string TimeOfDay()
        {
            if (isDay) { return "Day"; }
            else { return "Night"; }
        }
    }
}