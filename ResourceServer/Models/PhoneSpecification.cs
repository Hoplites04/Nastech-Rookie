namespace ResourceServer.Models
{
    public class PhoneSpecification
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        //* ===== Information =====
        public string Origin { get; set; } // e.g., "China"
        public DateTime ReleaseDate { get; set; } // e.g., "2023-09-01"
        public int WarrantyPeriodMonths { get; set; } // e.g,. "24 months"

        //* ===== Design & Weight =====
        public string Dimensions { get; set; }    // e.g., "158.2 x 77.9 x 7.4 mm"
        public string Weight { get; set; }      // e.g., "218g"
        public string WaterResistance { get; set; } // e.g., "IP68"
        public string FrameMaterial { get; set; }   // e.g., "Titanium"
        public string BackMaterial { get; set; }    // e.g., "Tempered glass"

        //* ===== Processor (CPU) =====
        public string Processor { get; set; } // e.g., "Snapdragon 888"
        public int Cores { get; set; } // e.g., "8 cores"
        public string RAM { get; set; } // e.g., "12 GB"

        //* ===== Display (Screen) =====
        public string ScreenSize { get; set; } // e.g., "6.9 inch"
        public string ScreenTechnology { get; set; } // e.g., "Dynamic AMOLED 2X"
        public string ScreenStandard { get; set; } // e.g., "2K"
        public string ScreenResolution { get; set; } // e.g., "3120 x 1440 Pixels"
        public string RefreshRate { get; set; } // e.g., "120 Hz"
        public string GlassMaterial { get; set; } // e.g., "Corning Gorilla Armor 2"
        public string Brightness { get; set; } // e.g., "2600 nits"

        //* ===== External Storage =====
        public string ContactStorage { get; set; }       // e.g., "No limit"
        public string ExternalMemoryCard { get; set; }   // e.g., "Not available"

        //* ===== Supported Connection and Sims =====
        public int SimSlots { get; set; }                      // e.g., 2
        public string SimType { get; set; }                    // e.g., "Nano SIM"
        public string SupportedNetworks { get; set; }          // e.g., "5G, 4G, 3G, 2G"
        public string Port { get; set; }                        // e.g., "1 Type C"
        public string Wifi { get; set; }                        // e.g., "Wi-Fi 6"
        public string Gps { get; set; }                         // e.g., "GPS, A-GPS, GLONASS, BDS, GALILEO"
        public string Bluetooth { get; set; }                   // e.g., "Bluetooth 5.2"
        public string OtherConnections { get; set; }            // e.g., "NFC, Infrared, USB OTG"

        //* ===== Battery ===
        public string BatteryType { get; set; }            // e.g., "Lithium-ion"
        public string BatteryCapacity { get; set; }        // e.g., "5000 mAh"
    }
}