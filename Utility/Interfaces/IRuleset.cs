namespace Utility
{
    public interface IRuleset
    {
        public string Name { get; }
        public string RulesetName { get; }
        public string ImageSource { get; }
        public string LogoSource { get; }
        public string FormResource { get; }
        public string Instructions { get; }
        public string NewCharacter();
        public ICharacter SaveCharacter(string data);
        public string LoadCharacter(ICharacter character);
    }
}
