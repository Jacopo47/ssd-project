import chartOrders
import arima_forecast
import sarimax
import mlForecasting
from flask import Flask, request

from arima_forecast import CustomerNotFound

app = Flask(__name__)

path_to_directory = './'
path_to_sqlite = '../ordiniMI2019.sqlite'

mlForecasting.model = mlForecasting.rnn_forecasting_model('../ordiniMI2018.sqlite', mlForecasting.scaler)


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


@app.route("/api/prevision/<customer>/arima", methods=['GET'])
def arima_prevision_on_customer(customer):
    try:
        return arima_forecast.get_arima_prediction_as_image(path_to_sqlite, customer)
    except CustomerNotFound:
        return "Customer not found", 404


@app.route("/api/prevision/<customer>/sarimax", methods=['GET'])
def sarimax_prevision_on_customer(customer):
    try:
        return sarimax.sarimax_forecast(path_to_sqlite, customer)
    except CustomerNotFound:
        return "Customer not found", 404


@app.route("/api/prevision/<customer>/ml", methods=['GET'])
def ml_forecasting_on_customer(customer):
    try:
        return mlForecasting.rnn_forecasting_predict(mlForecasting.model, mlForecasting.scaler, path_to_sqlite, customer)
    except CustomerNotFound:
        return "Customer not found", 404
