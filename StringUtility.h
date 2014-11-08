#pragma once
// A collection of useful string utility functions

#include "stdafx.h"

class StringUtility
{
public:
    // Splits a string into substrings, optionally including empty tokens if present.
    static void Split(const std::string& stringToSplit, char delimator, bool excludeEmpty, std::vector<std::string>& splitStrings);

    // Loads a file as a really big string; returns true on success.
    static bool LoadString(const std::string& filename, std::string& result);
};