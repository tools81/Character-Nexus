﻿using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transformers
{
    public class Ruleset : IRuleset
    {
        public string Name => "Transformers";

        public string RulesetName => "Ruleset.Transformers";

        public string ImageSource => "https://characternexus.blob.core.windows.net/resources/card_transformers.jpg";

        public string LogoSource => "https://characternexus.blob.core.windows.net/resources/logo_transformers.png";

        public string NewCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
