# This is a server app for my university project

from flask import Flask, render_template
import pymysql

app = Flask(__name__)
app.config['MYSQL_HOST'] = 'www-lab.ms.mff.cuni.cz'
app.config['MYSQL_USER'] = 'hartmaj2'
app.config['MYSQL_PASSWORD'] = 'mahZ4dee7uyioc4zei8Wah4nehiPie'
app.config['MYSQL_DB'] = 'hartmaj2'

requests_processed = 0
connection = pymysql.connect(host=app.config['MYSQL_HOST'],user=app.config['MYSQL_USER'],password=app.config['MYSQL_PASSWORD'],database=app.config['MYSQL_DB'])

@app.route('/')
def print_table():
    global requests_processed
    cursor = connection.cursor()
    cursor.execute("SELECT * FROM prihlasky")
    data = cursor.fetchall()
    requests_processed += 1
    print(data)
    cursor.close()
    return render_template('index.html',data=data)

@app.route('/requests')
def show_requests():
    return f"I have processed {requests_processed} requests"

@app.route('/reconnect')
def reconnect():
    global connection
    connection = pymysql.connect(host=app.config['MYSQL_HOST'],user=app.config['MYSQL_USER'],password=app.config['MYSQL_PASSWORD'],database=app.config['MYSQL_DB'])
    return "Reconnected"

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
