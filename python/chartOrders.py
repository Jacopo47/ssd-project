# Outputs a line chart of the time series of the orders of the selected customers.
# To be called via command line / C# (PythonRunner).
import os
import warnings

import common
import matplotlib.pyplot as plt
from cycler import cycler
from matplotlib import style


def get_prevision_as_image(path_to_directory, path_to_sqlite, customers_array):
    local_path = path_to_directory
    os.chdir(local_path)

    # Suppress all kinds of warnings (this would lead to an exception on the client side).
    warnings.simplefilter("ignore")

    # Preconfig plotting style, line colors and chart size.
    style.use('ggplot')
    plt.figure(figsize=(7, 5))
    plt.rc('axes', prop_cycle=(cycler('color', common.COLOR_MAP)))

    # parse command line arguments
    db_path = path_to_sqlite
    customers = customers_array

    # Get the orders from the database.
    dfs = common.get_orders(db_path, customers)

    # Draw a line to the chart for every single customer.
    for df in dfs:
        x = df['quant']
        y = df['time']
        plt.plot(y, x, linewidth=1)

    plt.xlabel('Mesi')
    plt.ylabel('Quant')

    # Finally, print the chart as base64 string to the console.
    return common.get_figure(plt.gcf())
