import common
import matplotlib.pyplot as plt
from flask import jsonify
from statsmodels.tools.eval_measures import rmse
from statsmodels.tsa.statespace.sarimax import SARIMAX


def sarimax_forecast(path_to_sqlite, customer):
    plt.figure()
    # Import data
    df = common.load_orders(path_to_sqlite, "'" + customer + "'")

    plt.title('Sarimax forecast - ' + customer, color='black')

    # Forecast next 3 months
    n_forecast = 3

    train = df.quant[0:-n_forecast]
    test = df.quant[-n_forecast:]

    ds = train
    sarima_model = SARIMAX(ds, order=(0, 2, 2), seasonal_order=(0, 1, 0, 12))
    sfit = sarima_model.fit()

    forewrap = sfit.get_forecast(steps=3)
    forecast_ci = forewrap.conf_int()
    forecast_val = forewrap.predicted_mean
    plt.plot(ds.values, label='Training set')
    plt.plot(test, label='Test set', color='green')
    plt.fill_between(forecast_ci.index,
                     forecast_ci.iloc[:, 0],
                     forecast_ci.iloc[:, 1], color='k', alpha=.25)

    plt.plot(forecast_val, label='Forecast')
    plt.xlabel('time')
    plt.ylabel('sales')
    plt.legend()

    metrics = common.forecast_accuracy(forecast_val.values, test)

    forecast = forecast_val.values.tolist()

    return jsonify(
        metrics=metrics,
        forecasts=forecast,
        image=common.get_figure(plt.gcf()).decode('utf-8')
    )
