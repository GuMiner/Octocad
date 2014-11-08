#include "stdafx.h"
#include "StringUtility.h"

// Splits a string into substrings, optionally including empty tokens if present.
void StringUtility::Split(const std::string& stringToSplit, char delimator, bool excludeEmpty, std::vector<std::string>& splitStrings)
{
    splitStrings.clear();
    std::stringstream stringStream(stringToSplit);
        
    std::string item;
    while (std::getline(stringStream, item, delimator))
    {
        if (!excludeEmpty || (excludeEmpty && !item.empty()))
        {
            splitStrings.push_back(item);
        }
    }
}

// Loads a file as a really big string; returns true on success.
bool StringUtility::LoadString(const std::string& filename, std::string& result)
{
    std::ifstream file (filename.c_str());
    if (!file) 
    {
        return false;
    }
        
    std::stringstream stream;
    stream << file.rdbuf();
    result = stream.str();
    
    file.close();
    return true;
}