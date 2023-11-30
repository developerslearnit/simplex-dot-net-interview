namespace FilterAndRank
{        
    public record Person(long Id, string Name);
    
    public record CountryRanking(long PersonId, string Country, int Rank);
     
    public record RankedResult(long PersonId, int Rank);
}
