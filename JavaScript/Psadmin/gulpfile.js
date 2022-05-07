"use strict";

var gulp = require("gulp");
var connect = require("gulp-connect");
var open = require("gulp-open");
var browserify = require("browserify");
var reactify = require("reactify");
var source = require("vinyl-source-stream");
var concat = require("gulp-concat");
var lint = require("gulp-eslint");

var config = {
    port: 9005,
    devBaseUrl: "http://localhost",
    path: {
        html: "./src/*.html",
        js: "./src/**/*.js",
        dist: "./dist",
        mainJs: "./src/main.js",
        images: "./src/images/*",
        css: [
            "node_modules/bootstrap/dist/css/bootstrap.min.css",
            "node_modules/bootstrap/dist/css/bootstrap-theme.min.css",
            "node_modules/toastr/toastr.css"
        ]
    }
}

//Start dev server
gulp.task("connect", function(){
    connect.server({
        root: ["dist"],
        port: config.port,
        base: config.devBaseUrl,
        livereload: true
    });
});

gulp.task("open", ["connect"], function(){
    gulp.src("dist/index.html")
        .pipe(open({ url: config.devBaseUrl + ":" + config.port + "/" }));
});

gulp.task("html", function(){
    gulp.src(config.path.html)
        .pipe(gulp.dest(config.path.dist))
        .pipe(connect.reload())
});

gulp.task("js", function(){
    browserify(config.path.mainJs)
        .transform(reactify)
        .bundle()
        .on("error", console.error.bind(console))
        .pipe(source("bundle.js"))
        .pipe(gulp.dest(config.path.dist + "/scripts"))
        .pipe(connect.reload());
});

gulp.task("css", function(){
    gulp.src(config.path.css)
        .pipe(concat("bundle.css"))
        .pipe(gulp.dest(config.path.dist + "/css"));
});

gulp.task("images", function(){
    gulp.src(config.path.images)
        .pipe(gulp.dest(config.path.dist + "/images"))
        .pipe(connect.reload());

        gulp.src("./src/favicon.ico")
            .pipe(gulp.dest(config.path.dist));
});

gulp.task("watch", function(){
    gulp.watch(config.path.html, ["html"]);
    gulp.watch(config.path.js, ["js"/*, "lint"*/])
});

/*
gulp.task("lint", function(){
    return gulp.src(config.path.js)
        .pipe(lint({config: "eslint.config.json"}))
        .pipe(lint.format());
});
*/
gulp.task("default", ["html", "js", "css", "images", "open", "watch"]);