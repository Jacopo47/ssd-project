import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import FunctionsIcon from '@material-ui/icons/Functions';
import SupervisorAccountIcon from '@material-ui/icons/SupervisorAccount';
import TimelineIcon from '@material-ui/icons/Timeline';
import TrackChangesIcon from '@material-ui/icons/TrackChanges';



export const ListItemKeys = {
    counter: 'counter',
    fetchData: 'fetch-data',
    prevision: 'prevision',
    optimize: 'optimize'
};

export const mainListItems = (onButtonClicked) => (
    <div>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.counter)}>
            <ListItemIcon>
                <FunctionsIcon />
            </ListItemIcon>
            <ListItemText primary="Counter" />
        </ListItem>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.fetchData)}>
            <ListItemIcon>
                <SupervisorAccountIcon />
            </ListItemIcon>
            <ListItemText primary="Fetch Data" />
        </ListItem>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.prevision)}>
            <ListItemIcon>
                <TimelineIcon />
            </ListItemIcon>
            <ListItemText primary="Prevision" />
        </ListItem>
        <ListItem button onClick={() => onButtonClicked(ListItemKeys.optimize)}>
            <ListItemIcon>
                <TrackChangesIcon />
            </ListItemIcon>
            <ListItemText primary="Optimize" />
        </ListItem>
    </div>
);