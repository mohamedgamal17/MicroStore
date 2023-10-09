module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },

    mappings: {
        "@node_modules/summernote/dist/**/*": "@libs/summernote/",
        "@node_modules/admin-lte/dist/**/*": "@libs/admin-lte/",
        "@node_modules/animate.css/**/*": "@libs/animate.css/",
        "@node_modules/lazysizes/**/*":"@libs/lazysizes/",
        "@node_modules/ion-rangeslider/**/*":"@libs/ion-rangeslider/",
        "@node_modules/slick-carousel/slick/**/*": "@libs/slick/",
        "@node_modules/star-rating.js/**/*": "@libs/star-rating.js/",
        "@node_modules/chart.js/**/*": "@libs/chart.js/",
        "@node_modules/chartjs-adapter-luxon/**/*":"@libs/chartjs-adapter-luxon/"
    }
}