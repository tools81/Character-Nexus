﻿using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerewolfTheApocalypse
{
    public class Ruleset : IRuleset
    {
        public string Name => "Werewolf: The Apocalypse";

        public string RulesetName => "Ruleset.WerewolfTheApocalypse";

        public string ImageSource => "";

        public string LogoSource => "";

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
