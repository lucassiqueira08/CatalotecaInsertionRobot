namespace CatalotecaInsertionRobot.app.src
{
  public class MysqlData
  {
    public static void CSVToMySQL()
    {
      string ConnectionString = "server=192.168.1xxx";
      string Command = "INSERT INTO User (FirstName, LastName ) VALUES (@FirstName, @LastName);";
      using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
      {
        mConnection.Open();
        using (MySqlTransaction trans = mConnection.BeginTransaction())
        {
          using (MySqlCommand myCmd = new MySqlCommand(Command, mConnection, trans))
          {
            myCmd.CommandType = CommandType.Text;
            for (int i = 0; i <= 99999; i++)
            {
              //inserting 100k items
              myCmd.Parameters.Clear();
              myCmd.Parameters.AddWithValue("@FirstName", "test");
              myCmd.Parameters.AddWithValue("@LastName", "test");
              myCmd.ExecuteNonQuery();
            }
            trans.Commit();
          }
        }
      }
    }
  }
}






// const mysqlData = {};

// const patterns = require('../logger/patterns');

// mysqlData.__getDB = async options => {
// 	const mysql = require('mysql');
// 	patterns.tryOpenConnection('mysqlData.__getDB', options);
// 	const connection = mysql.createConnection(options);
// 	await connection.connect(err => {
// 		if (err) {
// 			patterns.functionError('mysqlData.__getDB', options);
// 		}
// 	});
// 	patterns.successfullyOpenedConnection('mysqlData.__getDB', options);
// 	return connection;
// };

// mysqlData.run = async params => {
// 	patterns.functionInput('mysqlData.run', params);
// 	const connection = await mysqlData.__getDB(params.auth);
// 	return new Promise(async (resolve, reject) => {
// 		try {
// 			try {
// 				connection.beginTransaction(err => {
// 					patterns.functionInput('mysqlData.run.beginTransaction', params);
// 					if (err) {
// 						patterns.functionError(
// 							'[mysqlData.run.beginTransaction] Error when begin transaction'
// 						);
// 						throw err;
// 					}
// 					const response = [];
// 					params.querys.forEach(query => {
// 						const { sql, values } = query;
// 						connection.query(sql, [values], (queryError, result) => {
// 							if (queryError) {
// 								connection.rollback(() => {
// 									patterns.functionError(
// 										'[mysqlData.run.query] Error when executing the query'
// 									);
// 									throw queryError;
// 								});
// 							}
// 							response.push(result);
// 						});
// 					});
// 					connection.commit(error => {
// 						if (error) {
// 							connection.rollback(() => {
// 								patterns.functionError(
// 									'[mysqlData.run.commit] Error when commit changes'
// 								);
// 								throw error;
// 							});
// 						}
// 						resolve(response);
// 					});
// 				});
// 			} catch (error) {
// 				patterns.functionError('mysqlDatarun.run', error);
// 				reject(error);
// 			}
// 		} catch (err) {
// 			patterns.functionError('mysqlData.run', err);
// 			reject(err);
// 		}
// 	}).finally(() => {
// 		connection.end();
// 		patterns.successfullyClosedConnection('mysqlData.run.beginTransaction');
// 	});
// };

// mysqlData.validUser = authData => {
// 	patterns.functionInput('validUser', authData);
// 	const validation = `SHOW GRANTS FOR ${authData.user}`;
// 	return new Promise(async (resolve, reject) => {
// 		const connection = await mysqlData.__getDB({
// 			user: authData.user,
// 			password: authData.password,
// 			host: authData.host,
// 			database: authData.database,
// 			port: authData.port,
// 		});
// 		connection.query(validation, (error, results, fields) => {
// 			try {
// 				if (results) {
// 					resolve({ msg: 'AuthenticatedUser' });
// 					return;
// 				}
// 				if (error) {
// 					patterns.functionError('validUser', error);
// 					reject({ msg: 'UnauthorizedUser' });
// 					return;
// 				}
// 				patterns.functionError('validUser', error);
// 				reject({ msg: 'UnauthorizedUser' });
// 			} catch (err) {
// 				patterns.functionError('validUser', error);
// 				reject({ msg: 'UnauthorizedUser' });
// 			}
// 		});
// 	});
// };

// module.exports = mysqlData;
