/****************************************
* File: asasmtest.h						*
* Created: 6/13/2013					*
* Author: James Hammond					*
* Comments: Doxygen						*
****************************************/

/*! Library for random functions/classes until they have a real home. */
#ifndef TOOLBOX_H
#define TOOLBOX_H

#include <string>
#include <sstream>

namespace toolbox {
	//Function Prototypes
	std::string IntToString(int inToConvert);
	const char* StringtoChar(std::string convert);

	//Function Definitions
	std::string IntToString(int inToConvert) {
		std::ostringstream ss;
		ss << inToConvert;
		return ss.str();
	}

	const char* StringtoChar(std::string convert) {
		const char* converted = convert.c_str();
		return converted;
	}
}

#endif