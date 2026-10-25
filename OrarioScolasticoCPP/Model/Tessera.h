#pragma once

#include <string>

class Tessera {
private:
    std::string materia;
    std::string aula;

public:
    Tessera(std::string materia, std::string aula);

    std::string GetMateria();
    std::string GetAula();
};
