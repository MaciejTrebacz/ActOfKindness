import React, { useEffect, useState } from 'react';
import { MyEvent } from "../../../app/models/Events/myEvent";
import { Segment, Grid, Header, Item, Icon, Image, Button } from "semantic-ui-react";
import { observer } from 'mobx-react-lite';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { useStore } from '../../../app/stores/store';

interface Props {
    myEvent: MyEvent
}

function EventDetails({myEvent}:Props) {
    const { eventStore } = useStore();
    const { deleteEvent } = eventStore;
    const { id } = useParams();
    const navigate = useNavigate();
    const [event, setEvent] = useState<MyEvent>({
        id: '',
        userId: '',
        createdTime: '',
        localization: '',
        title:	'Oooooops',
        description: 'Something went wrong or just loading',
        startingDate: '',
        endingDate: '',
        done: '',
        latitude: '',
        longitude: '',
        moderated: '',
        type: '',
        image: '',
        participants:[],
        createdBy: {
            username: '',
            nickname: '',
        },
    });

    useEffect(() => {
    const loadEventDetails = async () => {
        try {
            if (id) {
                await eventStore.getParticipants(id)
                const userName = await eventStore.getUser(id, event.userId)
                if (myEvent){
                    setEvent(myEvent);
                }
            }
        } catch (error) {
            console.error('Error loading event details:', error);
        }
        return ()=>{
            eventStore.selectedEvent = undefined
        }
    };
    loadEventDetails();
    }, [eventStore, id]);

    const handleDelete = async () => {
        if (event.id) {
            await deleteEvent(event.id);
            navigate('/events');
        }
    };

    return (
        <>
            <Segment.Group>
                <Segment>
                    <Item><Header textAlign='center' size='large'>{event.title}</Header></Item>
                </Segment>
                <Segment.Group>
                    <Segment>
                        {event.description}
                        <Image src={event.image} size='large' />
                    </Segment>
 
                    <Segment>
                        <Segment>
                            <Grid verticalAlign={'middle'}>
                                <Grid.Column width={5}>
                                    <span>
                                        <Icon name='calendar' style={{marginBottom: 10}} size='large' color='teal'/>
                                        Start date: {event.startingDate.slice(0,10)}
                                    </span>
                                </Grid.Column>
                                <Grid.Column width={6}>
                                    <Icon name='calendar' style={{marginBottom: 10}} size='large' color='teal'/>
                                    <span>
                                        End date: {event.endingDate.slice(0,10)}
                                    </span>
                                </Grid.Column>
                            </Grid>
                        </Segment>
                        <Segment>
                            <Grid verticalAlign={'middle'}>
                                <Grid.Column width={15}>
                                    <span>
                                        <Icon name='address card' style={{marginBottom: 6}} size='large' color='teal'/>
                                        localization: {event.localization}
                                    </span>
                                </Grid.Column>
                            </Grid>
                        </Segment>
                        <Segment>
                            <Grid verticalAlign={'middle'}>
                                <Grid.Column width={15}>
                                    <span>
                                        <Icon style={{marginBottom: 10}} name='user' size='large' color='teal'/>
                                        Created by: {event.createdBy.nickname}
                                    </span>
                                </Grid.Column>
                            </Grid>
                        </Segment>
                        <Segment>
                            <Grid verticalAlign={'middle'}>
                                <Grid.Column width={15}>
                                    <span>
                                        <Icon name='calendar' style={{marginBottom: 10}} size='large' color='teal'/>
                                        Created on: {event.createdTime.slice(0,10)}
                                    </span>
                                </Grid.Column>
                            </Grid>
                        </Segment>

                        <Segment>
                            <Grid verticalAlign={'middle'}>
                                <Grid.Column width={15}>
                                    <Button as={Link}
                                        floated={"right"}
                                        to={`/eventDetails/${event.id}`}
                                        color={"orange"}
                                        content={"Edit"}
                                        style={{marginLeft: 10}}
                                    ></Button>

                                    <Button as={Link}
                                        floated={"right"}
                                        onClick={handleDelete}
                                        color={"red"}
                                        content={"Delete"}
                                    ></Button>
                                  
                                    <Button as={Link}
                                        floated={"right"}
                                        color={'teal'}
                                        onClick={()=>eventStore.joinEvent(event.id)}
                                        content={"Join Event!"}/>
                                    </Grid.Column>
                                  
                            </Grid>
                        </Segment>
                    </Segment>
                </Segment.Group>
            </Segment.Group>
        </>
    );  
}

export default observer(EventDetails);