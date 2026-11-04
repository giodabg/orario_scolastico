cmake_minimum_required(VERSION 3.12)
project(OrarioDocenteCPP LANGUAGES CXX)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
add_executable(OrarioDocenteCPP
    main.cpp
    Model/Tessera.cpp
    Model/Posizione.cpp
    Model/Orario.cpp
    Engine/ValidatoreOrario.cpp
    Engine/MotoreScambi.cpp
    DatiEsempio.cpp
    InterfacciaConsole.cpp
)
