"""
Common functions and a colormap for the line charts.
"""

# +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

import base64
import io

import pandas as pd
import numpy as np
from sqlalchemy import create_engine


# ---------------------------- read from sqlite database
def load_orders(db, cust):
    sql = "SELECT time, quant FROM ordini WHERE customer IN ({})".format(cust)

    engine = create_engine('sqlite:///' + db)

    df_all_orders = pd.read_sql(sql, engine, index_col='time')

    if df_all_orders.size == 0:
        raise CustomerNotFound()

    return df_all_orders


def load_stock_data(db, tickers, start_date, end_date):
    """
    Loads the stock data for the specified ticker symbols, and for the specified date range.
    :param db: Full path to database with stock data.
    :param tickers: A list with ticker symbols.
    :param start_date: The start date.
    :param end_date: The start date.
    :return: A list of time-indexed dataframe, one for each ticker, ordered by date.
    """

    SQL = "SELECT * FROM Quotes WHERE TICKER IN ({}) AND Date >= '{}' AND Date <= '{}'" \
        .format(tickers, start_date, end_date)

    engine = create_engine('sqlite:///' + db)

    df_all = pd.read_sql(SQL, engine, index_col='Date', parse_dates='Date')
    df_all = df_all.round(2)

    result = []

    for ticker in tickers.split(","):
        df_ticker = df_all.query("Ticker == " + ticker)
        result.append(df_ticker)

    return result


# ------------------------------ Accuracy metrics
def forecast_accuracy(forecast, actual):
    mape = np.mean(np.abs(forecast - actual) / np.abs(actual))  # MAPE
    me = np.mean(forecast - actual)  # ME
    mae = np.mean(np.abs(forecast - actual))  # MAE
    mpe = np.mean((forecast - actual) / actual)  # MPE
    rmse = np.mean((forecast - actual) ** 2) ** .5  # RMSE
    corr = np.corrcoef(forecast, actual)[0, 1]  # corr
    mins = np.amin(np.hstack([forecast[:, None],
                              actual[:, None]]), axis=1)
    maxs = np.amax(np.hstack([forecast[:, None],
                              actual[:, None]]), axis=1)
    minmax = 1 - np.mean(mins / maxs)  # minmax
    return ({'mape': mape, 'me': me, 'mae': mae,
             'mpe': mpe, 'rmse': rmse,
             'corr': corr, 'minmax': minmax})


def get_orders(db, customers):
    SQL = "SELECT * FROM ordini WHERE customer IN ({})" \
        .format(customers)

    engine = create_engine('sqlite:///' + db)

    df_all_orders = pd.read_sql(SQL, engine, index_col='id')

    result = []

    for cust in customers.split(","):
        df_order = df_all_orders.query("customer == " + cust)
        result.append(df_order)

    return result


# +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


def get_figure(fig):
    """
    Converts a figure (as created e.g. with matplotlib or seaborn) to a png image and this
    png subsequently to a base64-string, then prints the resulting string to the console.
    """

    buf = io.BytesIO()
    fig.savefig(buf, format='png')
    return base64.b64encode(buf.getbuffer())


# +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

# Custom colormap that is used with line charts
COLOR_MAP = [
    'blue', 'orange', 'green', 'red', 'purple', 'brown', 'pink', 'gray', 'olive', 'cyan',
    'darkblue', 'darkorange', 'darkgreen', 'darkred', 'rebeccapurple', 'darkslategray',
    'mediumvioletred', 'dimgray', 'seagreen', 'darkcyan', 'deepskyblue', 'yellow',
    'lightgreen', 'lightcoral', 'plum', 'lightslategrey', 'lightpink', 'lightgray',
    'lime', 'cadetblue'
]


# +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


class CustomerNotFound(Exception):
    """Base class for other exceptions"""
    pass
