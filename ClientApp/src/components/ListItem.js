import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import DashboardIcon from '@material-ui/icons/Dashboard';
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart';
import BarChartIcon from '@material-ui/icons/BarChart';

export const ListItemKeys = {
    counter: 'counter',
    fetchData: 'fetch-data',
    prevision: 'prevision'
};

export const mainListItems = (onButtonClicked) => (
    <div>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.counter)}>
            <ListItemIcon>
                <DashboardIcon />
            </ListItemIcon>
            <ListItemText primary="Counter" />
        </ListItem>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.fetchData)}>
            <ListItemIcon>
                <ShoppingCartIcon />
            </ListItemIcon>
            <ListItemText primary="Fetch Data" />
        </ListItem>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.prevision)}>
            <ListItemIcon>
                <BarChartIcon />
            </ListItemIcon>
            <ListItemText primary="Prevision" />
        </ListItem>
    </div>
);