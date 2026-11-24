Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports NTSInformatica.CLN__STD
' WSS 439265 - verifica autoporting
<Serializable()>
Public Class CLDHHIDBC
    Inherits CLD__BASE


#Region "Proprietà e variabili"

    Private _strServerSQL As String
    Public Property strServerSQL() As String
        Get
            Return _strServerSQL
        End Get
        Set(ByVal value As String)
            _strServerSQL = value
        End Set
    End Property

    Private _strNomeDB As String
    Public Property strNomeDB() As String
        Get
            Return _strNomeDB
        End Get
        Set(ByVal value As String)
            _strNomeDB = value
        End Set
    End Property

    Private _strUtenteSQL As String
    Public Property strUtenteSQL() As String
        Get
            Return _strUtenteSQL
        End Get
        Set(ByVal value As String)
            _strUtenteSQL = value
        End Set
    End Property

    Private _strPswSQL As String
    Public Property strPswSQL() As String
        Get
            Return _strPswSQL
        End Get
        Set(ByVal value As String)
            _strPswSQL = value
        End Set
    End Property

#End Region


    Public Overrides Sub Init(Applic As CLE__APP)
        Try
            MyBase.Init(Applic)

            LeggiRegistro()

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub LeggiRegistro()
        Try
            strServerSQL = GetSettingBus("BSHHIDBC", "OPZIONI", ".", "ServerSQL", "", "", "")
            strUtenteSQL = GetSettingBus("BSHHIDBC", "OPZIONI", ".", "UserSQL", "", "", "")
            strPswSQL = GetSettingBus("BSHHIDBC", "OPZIONI", ".", "PswSQL", "", "", "")
            strNomeDB = GetSettingBus("BSHHIDBC", "OPZIONI", ".", "NomeDatabase", "", "", "")
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub


#Region ""

    Public Overridable Function GetDataDBCent(ByRef ds As DataSet) As Boolean

        Dim strSQL As String = ""
        Dim dtt As New DataTable

        Try

            ' ---------------------------------------------------------------
            ' tabella delle relazione delle distinte
            strSQL = "SELECT " &
                     "    BOM_ORDER,  " &
                     "    BOM_PARENT,  " &
                     "    BOMDB_DOC.CODE,  " &
                     "    BOMDB_DOC.DESC1,  " &
                     "    BOM_CHILD,  " &
                     "    figli.CODE,  " &
                     "    figli.DESC1,  " &
                     "    BOM_POS,  " &
                     "    BOM_QTY  " &
                     "  FROM  " &
                     "    [DBCENT_CLEMENTI_BOMDB].[dbo].[BOMDB_LINK]  " &
                     "    INNER JOIN BOMDB_DOC  " &
                     "            ON [BOMDB_LINK].BOM_PARENT = BOMDB_DOC.BOM_UID  " &
                     "    INNER JOIN BOMDB_DOC AS figli  " &
                     "            ON [BOMDB_LINK].BOM_CHILD = figli.BOM_UID  " &
                     "  ORDER  BY  " &
                     "    BOM_ORDER,  " &
                     "    BOM_PARENT,  " &
                     "    [BOMDB_LINK].BOM_POS ASC "
            dtt = ExecuteQuery(strSQL, "BOMDB_LINK")
            ds.Tables.Add(dtt)
            ' ---------------------------------------------------------------
            ' ---------------------------------------------------------------
            ' tabella anagrafica dei codici DBCent
            strSQL = "select * from BOMDB_DOC"
            dtt = ExecuteQuery(strSQL, "BOMDB_DOC")
            ds.Tables.Add(dtt)
            ' ---------------------------------------------------------------

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function


#End Region



#Region "Metodi per connessione\interrogazione database esterno"

    ''' <summary>
    ''' Esegui query di selezione NON asincrona
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <param name="strTableName"></param>
    ''' <returns></returns>
    Public Overridable Function ExecuteQuery(ByVal strSQL As String, ByVal strTableName As String) As DataTable
        Dim dtt As New DataTable
        Dim oConn As SqlConnection = Nothing
        Try
            ' ricavo il nome della tabella 
            'apro la connessione
            oConn = GetOpenedConnection()

            Using oConn
                Using oCmd As New SqlCommand(strSQL, oConn)
                    Using sr As SqlDataReader = oCmd.ExecuteReader()
                        ' popolo il dataset
                        dtt.TableName = strTableName
                        dtt.Load(sr)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
        Return dtt
    End Function


    ''' <summary>
    ''' Esegui query di selezione Asincrona
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <param name="strTableName"></param>
    ''' <returns></returns>
    Public Overridable Async Function ExecuteQueryAsync(ByVal strSQL As String, ByVal strTableName As String) As Task(Of DataTable)
        Dim dtt As New DataTable
        Dim oConn As SqlConnection = Nothing
        Try
            ' ricavo il nome della tabella 
            'apro la connessione
            oConn = Await GetOpenedConnectionAsync()

            Using oConn
                Using oCmd As New SqlCommand(strSQL, oConn)
                    Using sr As SqlDataReader = Await oCmd.ExecuteReaderAsync()
                        ' popolo il dataset
                        dtt.TableName = strTableName
                        dtt.Load(sr)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
        Return dtt
    End Function


    ''' <summary>
    ''' Crea e apre una nuova connessione. Metodo Asincrono
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Async Function GetOpenedConnectionAsync() As Threading.Tasks.Task(Of SqlConnection)
        Dim obj As SqlConnection = Nothing
        Try
            obj = New SqlConnection($"Server={strServerSql};Database={strNomeDb};UID={strUtenteSql};pwd={strPswSql};LANGUAGE=us_english")
            Await obj.OpenAsync()
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
        Return obj
    End Function


    ''' <summary>
    ''' Crea e apre una nuova connessione. Metodo NON asincrono
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function GetOpenedConnection() As SqlConnection
        Dim obj As SqlConnection = Nothing
        Try
            obj = New SqlConnection($"Server={strServerSql};Database={strNomeDb};UID={strUtenteSql};pwd={strPswSql};LANGUAGE=us_english")
            obj.Open()
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
        Return obj
    End Function

#End Region

End Class
