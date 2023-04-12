module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },

    mappings: {
        "@node_modules/summernote/dist/**/*": "@libs/summernote/",
        "@node_modules/admin-lte/dist/**/*" :"@libs/admin-lte/",
    }
}