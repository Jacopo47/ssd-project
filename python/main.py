import chartOrders
import arima_forecast
from flask import Flask, request

from arima_forecast import CustomerNotFound

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
    input_customers = request.get_json()['customers']

    if len(input_customers) == 0:
        return 'Customers not defined', 400

    customers = ','.join("'{0}'".format(w) for w in input_customers)

    return chartOrders.get_prevision_as_image(path_to_directory, path_to_sqlite, customers)


@app.route("/api/prevision/<customer>", methods=['GET'])
def arima_prevision_on_customer(customer):
    try:
        return arima_forecast.get_arima_prediction_as_image(path_to_sqlite, customer)
    except CustomerNotFound:
        return "Customer not found", 404
