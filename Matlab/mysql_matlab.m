function mysql_matlab()


patientID = 12;
sessionID = 13;
playlistID = 14;
exerciseID = 15;

error = 16;
csv_figure_name = strcat('dtw_', 'session', sessionID, '_playlist', playlistID, '_exercise', exerciseID, '.png');



%# JDBC connector path
javaaddpath('C:\Users\Public\mysql-connector-java-5.0.8\mysql-connector-java-5.0.8-bin.jar')
%# connection parameteres
host = 'localhost'; %MySQL hostname
user = 'root'; %MySQL username
password = 'magnidb1';%MySQL password
dbName = 'magni_db'; %MySQL database name
%# JDBC parameters
jdbcString = sprintf('jdbc:mysql://%s/%s', host, dbName);
jdbcDriver = 'com.mysql.jdbc.Driver';
conn = database(dbName, user , password, jdbcDriver, jdbcString);
if isconnection(conn)
    qry1 = sprintf('INSERT INTO magni_encounter(encounterID,patientID,sessionID)VALUES(sessionID,patientID,sessionID)');
    qry2 = sprintf('INSERT INTO magni_session(sessionID,exerciseID,playlistID,score,graphName)VALUES(sessionID,exerciseID,playlistID,error,csv_figure_name)');
    qry = sprintf('Select * From magni_session');
    exec(conn, qry1);
    exec(conn, qry2);
    rs = fetch(exec(conn, qry));
    alldata = get(rs, 'Data');
    display(alldata);
else
    display('MySql Connection Error');
    
end

end