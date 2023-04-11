When replacing/updating AmuletOfManyMinionsApi
* add the [Content(ConfigurationSystem.AllFlags)] attribute
* add to the AommMod getter: if (!ContentConfig.Instance.AommSupport) { return null; }
* ModSystem -> AssSystem