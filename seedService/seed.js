// Start with `npm run seed`

const mongoose = require('mongoose');
const { v4: uuid } = require('uuid');

const { connect, connection, Schema, model } = mongoose;

const MONGO_CONNECTION_STRING = 'mongodb://localhost:27017/pollDb';

const POLLS_TO_GENERATE = 10000;
const OPTIONS_PER_POLL = 5;
const VOTES_PER_OPTION = 2;

const CHUNK_SIZE = 1000;

const voteSchema = new Schema({
    _id: { type: String },
    Name: {type: String},
});

const optionSchema = new Schema({
    Title: { type: String },
    Value: { type: String },
    Votes: [voteSchema],
}, { _id : false })

const pollSchema = new Schema({
    Title : { type: String },
    ChannelId : { type: String },
    MessageTimestamp : { type: String },
    Date : Date,
    Options: [optionSchema],
    MultiSelect : Boolean,
    ShowVoters : Boolean,
    Locked : Boolean,
    Owner : { type: String },
});

const PollModel = model('Polls', pollSchema, 'Polls');

const initDatabase = async () => {
    await connect(MONGO_CONNECTION_STRING);
};

(async () => {
    await initDatabase();

    console.log('Starting seeding...');

    for (let i = 0; i < POLLS_TO_GENERATE; i += CHUNK_SIZE) {
        i !== 0 && console.log(`Inserted ${i} polls`);
        const iterationCount = Math.min(POLLS_TO_GENERATE - i, CHUNK_SIZE);
        await PollModel.insertMany(
            Array.from({length: iterationCount}, () => buildPoll())
        );
    }

    const pollCount = await PollModel.count({});

    console.log('Seeding completed!');
    console.log(`${pollCount} polls are currently in DB`);
})().then(async () => {
    await connection.close();
});

const buildVote = (userName = uuid()) => ({
    _id: uuid(),
    Name: userName,
});

const buildOption = () => ({
    Title: uuid(),
    Value: uuid(),
    Votes: Array.from({length: VOTES_PER_OPTION}, () => buildVote())
});

const buildPoll = () => ({
    Title: uuid(),
    ChannelId: uuid(),
    MessageTimestamp: new Date().getTime().toString(),
    Date: new Date(),
    Options: Array.from({length: OPTIONS_PER_POLL}, () => buildOption()),
    MultiSelect: false,
    ShowVoters: false,
    Locked: false,
    Owner: uuid(),
});