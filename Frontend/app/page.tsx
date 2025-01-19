"use client";

import { motion, AnimatePresence } from "framer-motion";
import { Github, Music2, Sparkles, Play, Pause, Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useState, useRef } from "react";
import { useToast } from "@/hooks/use-toast";

export default function Home() {
  const [username, setUsername] = useState("");
  const [showPlayer, setShowPlayer] = useState(false);
  const [isPlaying, setIsPlaying] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const { toast } = useToast();
  const apiUrl = "https://localhost:7029";

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!username) {
      toast({
        title: "Error",
        description: "Please enter a GitHub username",
        variant: "destructive",
      });
      return;
    }

    setIsLoading(true);
    try {
      //API REQUEST #1: Send GitHub username to backend
      const response = await fetch(
        apiUrl + `/api/Music?userName=${username}&rhytmPatternType=Sad`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
        }
      );

      if (!response.ok) throw new Error("Failed to generate music");

      const data = await response.json();
      console.log(data);

      const audioUrl = apiUrl + data.virtualPath;
      console.log(audioRef.current);

      if (audioRef.current) {
        console.log("girdim");
        audioRef.current.src = audioUrl;
      }

      setIsLoading(false);
      setShowPlayer(true);
      toast({
        title: "Success!",
        description: "Your music is ready to play",
      });
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to generate music. Please try again.",
        variant: "destructive",
      });
      setIsLoading(false);
    }

    // Simulate loading for 5 seconds
    setTimeout(() => {
      setIsLoading(false);
      setShowPlayer(true);
      toast({
        title: "Success!",
        description: "Your music is ready to play",
      });
    }, 1000);
  };

  const togglePlay = () => {
    // AUDIO PLAYBACK CONTROL
    if (audioRef.current) {
      if (isPlaying) {
        audioRef.current.pause();
      } else {
        audioRef.current.play();
      }
      setIsPlaying(!isPlaying);
      setIsPlaying(!isPlaying);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-600 via-pink-500 to-orange-400 flex items-center justify-center">
      <audio ref={audioRef} />
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.8 }}
        className="w-full max-w-md px-4"
      >
        <motion.div
          initial={{ scale: 0 }}
          animate={{ scale: 1 }}
          transition={{ type: "spring", stiffness: 260, damping: 20 }}
          className="flex justify-center mb-8 space-x-4"
        >
          <Music2 className="w-12 h-12 text-white animate-bounce" />
          <Github className="w-12 h-12 text-white animate-pulse" />
        </motion.div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.3 }}
          className="bg-white/10 backdrop-blur-lg rounded-xl p-8 shadow-xl"
        >
          <h1 className="text-3xl font-bold text-center text-white mb-2">
            Music & GitHub
          </h1>
          <p className="text-center text-white/80 mb-8">
            Enter your GitHub username to get started
          </p>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="relative">
              <motion.div
                whileHover={{ scale: 1.02 }}
                whileTap={{ scale: 0.98 }}
              >
                <Input
                  type="text"
                  placeholder="GitHub Username"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="bg-white/20 border-white/30 text-white placeholder:text-white/50 focus:ring-2 focus:ring-white/50"
                  disabled={isLoading}
                />
              </motion.div>
              <Sparkles className="absolute right-3 top-1/2 transform -translate-y-1/2 text-white/50 w-5 h-5" />
            </div>

            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
              <Button
                type="submit"
                className="w-full bg-white text-purple-600 hover:bg-white/90 transition-all disabled:opacity-50"
                disabled={isLoading}
              >
                {isLoading ? (
                  <>
                    <Loader2 className="w-5 h-5 mr-2 animate-spin" />
                    Generating Music...
                  </>
                ) : (
                  <>
                    <Github className="w-5 h-5 mr-2" />
                    Submit
                  </>
                )}
              </Button>
            </motion.div>
          </form>

          <AnimatePresence>
            {showPlayer && (
              <motion.div
                initial={{ opacity: 0, height: 0 }}
                animate={{ opacity: 1, height: "auto" }}
                exit={{ opacity: 0, height: 0 }}
                transition={{ duration: 0.5 }}
                className="mt-6"
              >
                <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                  className="bg-white/20 rounded-lg p-4 flex items-center justify-between"
                >
                  <div className="text-white font-medium">
                    Your GitHub Music
                  </div>
                  <Button
                    onClick={togglePlay}
                    variant="ghost"
                    size="icon"
                    className="text-white hover:bg-white/20"
                  >
                    {isPlaying ? (
                      <Pause className="w-6 h-6" />
                    ) : (
                      <Play className="w-6 h-6" />
                    )}
                  </Button>
                </motion.div>
              </motion.div>
            )}
          </AnimatePresence>
        </motion.div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.6 }}
          className="mt-8 text-center text-white/60 text-sm"
        >
          Discover the harmony between code and music
        </motion.div>
      </motion.div>
    </div>
  );
}
