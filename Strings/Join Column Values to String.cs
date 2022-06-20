class Strings
{
    static string string_JoinDataTableColumnValuesToString(DataTable dt, string columnName, string joinString, bool addToLast)
    {
        string output = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            output += dt.Rows[i][columnName].ToString() + joinString;
        }

        if (addToLast)
        {
            output = output.Trim();
        }
        else
        {
            output = string_RemoveLastCharackter(output.Trim());
        }


        return output;

    }

    static string string_RemoveLastCharackter(string thisString)
    {
        return thisString.Remove(thisString.Length - 1);
    }
}