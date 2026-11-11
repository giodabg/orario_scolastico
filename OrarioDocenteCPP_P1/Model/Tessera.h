#pragma once
#include <string>

class Tessera {
private:
    std::string materia;
    std::string classe;
    std::string aula;
public:
    Tessera(std::string materia, std::string classe, std::string aula);
    std::string GetMateria();
    std::string GetClasse();
    std::string GetAula();
};
