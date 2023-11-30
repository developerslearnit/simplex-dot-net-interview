**note:** *You are given more time than needed to solve the problem, so please read these instructions thoroughly. This project is setup for .NET 6. Please use your normal C# IDE/Editor for development.*

## Writing the filter and ranking function.

We have a collection of people (`Person` objects), with the following fields:

* `id` (long)
* `name` (string)

Some of those people will have ranking data for the single country in which they are registered (`CountryRanking` objects) in the form of:

* `personId` (long -> `id` in `Person` object)
* `country` (string)
* `rank` (integer)

All people are unique by the `id` and there are no duplicates.  There is at most one `CountryRanking` for a person.

Given data in this format, you are to implement a filtering and ranking function with the following method signature:

```c#
IList<RankedResult> FilterByCountryWithRank(
            IList<Person> people,
            IList<CountryRanking> rankingData,
            IList<string> countryFilter,
            int minRank, 
            int maxRank,
            int maxCount)
```

You can find an empty version of the function in `Ranking.cs`.  Please write your implementation there, but do not change the method signature in any way, or you will break the automatic verification of your solution. Related classes are located in the `Types.cs` file.

### Requirements

The following are the requirements for implementing this function:

1. People without ranking data should not be included
2. Persons should be filtered by a list of countries (`countryFilter`), so that only those persons in the specified `countryFilter` are returned.
    * All non-matching persons are filtered out.
    * Country names are not guaranteed to be normalized to any specific case (case insensitive).
3. Persons should be filtered by a range of rankings so that all persons are within the inclusive range of `rankFilter`.
4. Persons should be ordered by their ranking,
    * and when there is a tie, they should be ordered by the countries to match the same order (ordinal) as specified in the `countryFilter` list,
    * and any further tie should be broken by the order of the person's name (case insensitive).
5. The function should return at most `maxCount` items
    * **unless** this would divide a rank, and in that case more items should be returned so that all persons of the same rank are included.
6. The result of the function is the ordered list of `RankedResult` objects which only contain the person `id` and their original country `rank`.

*see clarifying examples below...*

### Tips

You are given extra time beyond what is expected for implementing this code, to ensure that you have time to:

* write clean code.
* write well-structured code.
* write idiomatic code.
* write reasonable optimal code for an input size of less than 500 persons and ranking data.
* write **unit tests** that cover the requirements (see `RankingTests.cs` for an example test case).

**All of these elements will be reviewed during evaluation** of your code.  Total implementation time is less of a consideration.

### Clarifying Examples

Some examples, for clarity:  (_note, these do not match the unit test data nor the actual format of the input nor results, but are here to help explain the requirements_)

Given this list of people and ranking data as input:

```text
Person:  Santiago, rank 1, country Aruba
Person:  Linda,    rank 2, country Aruba
Person:  Adena,    rank 1, country Zimbabwe
Person:  Marisma,  rank 2, country Zimbabwe
```

> calling the function with countries `["Zimbabwe", "Aruba"]`, this would order matches within the same rank by this same order.  "Zimbabwe" first, "Aruba" second.

```text
Person:  Adena,    rank 1, country Zimbabwe
Person:  Santiago, rank 1, country Aruba
Person:  Marisma,  rank 2, country Zimbabwe
Person:  Linda,    rank 2, country Aruba
```

> calling the function with countries `["Zimbabwe", "Aruba"]` and a `maxCount` of `3` could return 3 or more items to avoid breaking up a rank.

```text
Person:  Adena,    rank 1, country Zimbabwe
Person:  Santiago, rank 1, country Aruba
Person:  Marisma,  rank 2, country Zimbabwe  <--- don't stop here, we need all rank 2 people regardless of the max!
Person:  Linda,    rank 2, country Aruba
```

## Running tests

```
dotnet test
```

