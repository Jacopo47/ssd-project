import chartOrders
from flask import Flask, request

app = Flask(__name__)

path_to_directory = './'
path_to_sqlite = '../ordiniMI2018.sqlite'


@app.route("/user/<name>")
def hello(name):
    response = {
        "msg": "Hello, World!",
        "user": {
            "name": name,
            "surname": "no data"
        }
    }

    return response


@app.route("/api/prevision", methods=['POST'])
def prevision():
    customers = ','.join("'{0}'".format(w) for w in request.get_json()['customers'])

    return chartOrders.get_prevision_as_image(path_to_directory, path_to_sqlite, customers)
