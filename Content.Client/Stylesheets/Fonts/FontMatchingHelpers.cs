using Robust.Client.Graphics;

namespace Content.Client.Stylesheets.Fonts;

// The underlying representation of FontWidth and FontWeight are ushorts, so to force an ordering of certain values,
// I'm able to add ushort.MaxValue so that it'll go in the order I want.

/// <summary>
/// Helpers for matching a font face's parameters to the closest existing font face.
/// </summary>
/// <remarks>
/// These algorithms are based on W3C's CSS Fonts Module Level 3 and 4.
/// </remarks>
public static class FontMatchingHelpers
{
    /// <summary>
    /// Compares the width to the goal, such that the "closer" widths will have lower values.
    /// </summary>
    /// <param name="actual">A font face's actual width</param>
    /// <param name="goal">The desired width</param>
    /// <returns>How close the actual is to the desired</returns>
    /// <seealso href="https://www.w3.org/TR/css-fonts-3/#font-style-matching" />
    public static int ClosestWidth(FontWidth actual, FontWidth goal)
    {
        var distance = Math.Abs((int)actual - (int)goal);

        var order = goal switch
        {
            // If the goal is more condensed than normal, check more condensed ones first
            <= FontWidth.Normal when actual > goal => ushort.MaxValue,
            // If the goal is wider than normal, check wider ones first
            > FontWidth.Normal when actual < goal => ushort.MaxValue,
            _ => 0,
        };

        return distance + order;
    }

    /// <summary>
    /// Compares the slant to the goal, such that the "closer" slants will have lower values.
    /// </summary>
    /// <param name="actual">A font face's actual slant</param>
    /// <param name="goal">The desired slant</param>
    /// <returns>How close the actual is to the desired</returns>
    /// <seealso href="https://www.w3.org/TR/css-fonts-3/#font-style-matching" />
    public static int ClosestSlant(FontSlant actual, FontSlant goal)
    {
        // https://www.w3.org/TR/css-fonts-3/#font-style-matching
        return actual switch
        {
            // If italic, match oblique then normal
            FontSlant.Italic when goal == FontSlant.Oblique => 1,
            FontSlant.Italic when goal == FontSlant.Normal => 2,
            // If oblique, match italic then normal
            FontSlant.Oblique when goal == FontSlant.Italic => 1,
            FontSlant.Oblique when goal == FontSlant.Normal => 1,
            // If normal, match oblique then italic
            FontSlant.Normal when goal == FontSlant.Oblique => 2,
            FontSlant.Normal when goal == FontSlant.Italic => 2,
            // If we are the goal, then do it first
            _ => 0
        };
    }

    /// <summary>
    /// Compares the weight to the goal, such that the "closer" weights will have lower values.
    /// </summary>
    /// <param name="actual">A font face's actual weight</param>
    /// <param name="goal">The desired weight</param>
    /// <returns>How close the actual is to the desired</returns>
    /// <seealso href="https://www.w3.org/TR/css-fonts-4/#font-style-matching" />
    public static int ClosestWeight(FontWeight actual, FontWeight goal)
    {
        // We use version 4 as there's non-100 font weights in the FontWeight enum...
        var distance = Math.Abs((int)actual - (int)goal);

        switch ((int)goal)
        {
            // If our goal is within 400 and 500:
            // 1) start from the goal, go bolder until 500
            // 2) then, go lighter until 0
            // 3) finally, go bolder from 500
            case >= 400 and <= 500:
            {
                if ((int)actual >= (int)goal && (int)actual <= 500)
                    return distance;

                if ((int)actual < (int)goal)
                    return distance + ushort.MaxValue;

                return distance + ushort.MaxValue * 2;
            }
            // If our goal is lighter than 400:
            // 1) start from the goal, go lighter
            // 2) then, go bolder
            case < 400:
            {
                if ((int)actual <= (int)goal)
                    return distance;

                return distance + ushort.MaxValue;
            }
            // If our goal is bolder than 500:
            // 1) start from the goal, go bolder
            // 2) then, go lighter
            case > 500:
            {
                // First, check any greater than the goal.
                if ((int)actual >= (int)goal)
                    return distance;

                // Then, check any lower than the goal.
                return distance + ushort.MaxValue;
            }
        }
    }
}
