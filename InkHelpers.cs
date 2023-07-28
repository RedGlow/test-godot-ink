using System.Collections.Generic;

public static class InkHelpers
{
    private const string ObjectChoicePrefix = "-- ";

    /// <summary>
    /// Get the arguments of a tag, or <c>null</c> if the tag is not found.
    /// </summary>
    /// <param name="tags">The list of tags</param>
    /// <param name="tagName">The name of the tag to find</param>
    /// <returns>The list of arguments, or <c>null</c> if not found.</returns>
    public static string[] GetTagArguments(List<string> tags, string tagName)
    {
        foreach (var tag in tags)
        {
            var parts = tag.Split(":");
            if (parts[0] == tagName)
            {
                return parts[1..^0];
            }
        }
        return null;
    }

    /// <summary>
    /// Get the object this choice is expressing, or <c>null</c> if this choice
    /// is not expressing an object.
    /// </summary>
    /// <param name="choiceText">The text of the choice.</param>
    /// <returns>The object this choice is expressing, or <c>null</c> if this
    /// choice is not expressing an object.</returns>
    public static string ObjectChoiceName(string choiceText) =>
        choiceText.IndexOf(ObjectChoicePrefix) < 0 ?
            null :
            choiceText[ObjectChoicePrefix.Length..];

}
