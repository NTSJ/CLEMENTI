Imports System.Data
Imports NTSInformatica.CLN__STD

''' <summary>
''' Tipo di assicurazione: in base al tipo effettuo controlli diversi da codice
''' </summary>
<Serializable()>
Public Enum TipoAssicurazione

    Non_impostata = 0
    Assicurazione = 1
    Fido = 2
    Rating = 3
    Annullata = 99

End Enum


